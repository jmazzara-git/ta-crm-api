using Microsoft.Extensions.Localization;

namespace TACRM.Services.Localization
{
	public class LocalizerFactory : IStringLocalizerFactory
	{
		private readonly string _basePath;

		public LocalizerFactory(string basePath)
		{
			_basePath = basePath;
		}

		public IStringLocalizer Create(Type resourceSource)
		{
			return new Localizer(_basePath);
		}

		public IStringLocalizer Create(string baseName, string location)
		{
			return new Localizer(_basePath);
		}
	}
}
