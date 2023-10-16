using private_key_jwt_tool;

"Generating a private and public jwt information set for client authentication".ConsoleWriteLine();

var jwk = CryptoHelper.GenerateJsonWebKey();
var privateKeyJson = jwk.GeneratePrivateKeyJson();
var privateKeyJsonAppSettings = privateKeyJson.Replace("\"", "'");
var publicKeyJson = jwk.GeneratePublicKeyJson();
var publicKeyJsonAppSettings = publicKeyJson.Replace("\"", "'");

"\n----- PRIVATE KEY SECTION -----".ConsoleWriteLine();
"The following represents the private key - to be used AT THE OAUTH CLIENT SIDE (e.g. OIDC RP)".ConsoleWriteLine();
"Json (as it can be stored in e.g. Azure Key Vault):".ConsoleWriteLine();
privateKeyJson.ConsoleColorWriteLine(ConsoleColor.Cyan);
"appSettings compatible:".ConsoleWriteLine();
privateKeyJsonAppSettings.ConsoleColorWriteLine(ConsoleColor.DarkCyan);

"\n----- PUBLIC KEY SECTION -----".ConsoleWriteLine(); 
"The following represents the public key - to be used AT THE TOKEN SERVICE (CLIENT STORE)".ConsoleWriteLine();
"Json (as it can be stored in e.g. sql db):".ConsoleWriteLine();
publicKeyJson.ConsoleColorWriteLine(ConsoleColor.Green);

"appSettings compatible:".ConsoleWriteLine();

publicKeyJsonAppSettings.ConsoleColorWriteLine(ConsoleColor.DarkGreen);
"-----".ConsoleWriteLine();

var autoSave = args.Any(a=> a.Equals("-f"));
var autoOverwrite = args.Any(a => a.Equals("-o"));

var privateFile = $"{jwk.Kid}_private.json";
var publicFile = $"{jwk.Kid}_public.json";

var save = autoSave;
if (!save)
{
	"Press 's' to save the files as json (or any other key to skip)".ConsoleWriteLine();
    var k = Console.ReadKey(true);
	save = k.KeyChar == 's';

    $"For next time, you can pass -f as command argument, to write the json to files: {privateFile} and {publicFile} (prefix changes)"
        .ConsoleColorWriteLine(ConsoleColor.DarkYellow);
}

if (save && (File.Exists(privateFile) || File.Exists(publicFile)) && !autoOverwrite)
{
	"Files already exist - press 'y' to override (or any other key to abort)".ConsoleColorWriteLine(ConsoleColor.Yellow);
	var k = Console.ReadKey(true);
	save = k.KeyChar == 'y';

	"For next time, you can pass -o as command argument, to overwrite existing files.".ConsoleColorWriteLine(ConsoleColor.DarkYellow);
}

if (save)
{
	File.WriteAllText(privateFile, privateKeyJson);
	File.WriteAllText(publicFile, publicKeyJson);
	$"Files written to {privateFile} and {publicFile}".ConsoleColorWriteLine(ConsoleColor.Yellow);
}
else
{
    "Files NOT written to disk as save option was not found or manually skipped".ConsoleColorWriteLine(ConsoleColor.Yellow);
}

"".ConsoleWriteLine(resetColor: true); 
"All done - Live long and prosper".ConsoleWriteLine();

if (!autoSave)
{
	"Press enter to exit".ConsoleWriteLine();
    Console.ReadLine();
}