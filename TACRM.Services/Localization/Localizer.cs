using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Globalization;

namespace TACRM.Services.Localization
{
	public class Localizer : IStringLocalizer
	{
		private readonly string _basePath;
		private readonly ConcurrentDictionary<string, Dictionary<string, string>> _localizationCache = new();

		public Localizer(string basePath)
		{
			_basePath = basePath;
			LoadLocalizations();
		}

		private void LoadLocalizations()
		{
			var supportedCultures = new[] { "en", "es" };

			foreach (var culture in supportedCultures)
			{
				var filePath = Path.Combine(_basePath, $"messages.{culture}.txt");

				if (!File.Exists(filePath))
					continue;

				var localizationDict = File.ReadLines(filePath)
					.Where(line => line.Contains('='))
					.Select(line => line.Split(new[] { '=' }, 2))
					.ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

				_localizationCache[culture] = localizationDict;
			}
		}

		public LocalizedString this[string name]
		{
			get
			{
				var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
				if (_localizationCache.TryGetValue(culture, out var translations) && translations.TryGetValue(name, out var value))
				{
					return new LocalizedString(name, value, resourceNotFound: false);
				}

				return new LocalizedString(name, name, resourceNotFound: true);
			}
		}

		public LocalizedString this[string name, params object[] arguments] =>
			new(name, string.Format(this[name].Value, arguments), resourceNotFound: false);

		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
			var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			if (_localizationCache.TryGetValue(culture, out var translations))
			{
				return translations.Select(kv => new LocalizedString(kv.Key, kv.Value, resourceNotFound: false));
			}

			return Enumerable.Empty<LocalizedString>();
		}
	}
}
