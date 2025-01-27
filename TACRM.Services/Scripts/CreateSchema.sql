-- Create schema for the project
CREATE SCHEMA tacrm;

-- Create database user
CREATE USER tacrm_user WITH PASSWORD 'SecureP@ss123';
GRANT CONNECT ON DATABASE tacrm TO tacrm_user;
GRANT USAGE ON SCHEMA tacrm TO tacrm_user;
GRANT ALL PRIVILEGES ON SCHEMA tacrm TO tacrm_user;