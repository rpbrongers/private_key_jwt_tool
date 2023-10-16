# private_key_jwt_tool
Little command line tool for generating copy/paste keypairs for use in Duende/OAuth2 client assertions (private key jwt)
Each run generates a new pair.
The tool can also produce files in addition to copy/paste output.

If you have no idea what problem this tools solves:
It helps with using this: https://docs.duendesoftware.com/identityserver/v6/tokens/authentication/jwt/

Don't forget: keep your private keys private, or else it simply ends up as a more complicated way of shared secrets :-)
