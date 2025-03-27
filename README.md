### Setting up Secrets via Powershell / Linux
``` 
*If using Linux, remove dollar ($) sign

Locally Hosted DB
sqllocaldb create 'CyberVaultDB'
sqllocaldb start 'CyberVaultDB'

$server_name='(localdb)\CyberVaultDB'
$database='CyberVaultDB'
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=$server_name;Database=$database;Trusted_Connection=True;MultipleActiveResultSets=True"

Online Hosted DB
$db_data_source="YOUR DATA SOURCE"
$db_user_id="USER ID"
$db_pwd='PASSWORD'

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=$db_data_source;
Initial Catalog=pet-management-db;User ID=$db_user_id;Password=$db_pwd;Trust Server Certificate=True"
		
JWT 

dotnet user-secrets set "JwtSettings:Key" "YOUR KEY"
dotnet user-secrets set "JwtSettings:Issuer" "CyberVault"
dotnet user-secrets set "JwtSettings:Audience" "CyberVaultAudience"

$jwt_key='YOUR KEY'
$jwt_issuer='CyberVault'
$jwt_audience='CyberVaultAudience'

dotnet user-secrets set "JwtSettings" "Key=$jwt_key; Issuer=$jwt_issuer; Audience=$jwt_audience"


Google Authentication
dotnet user-secrets set "Authentication:Google:Client" "YOUR_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_SECRET"
	
```