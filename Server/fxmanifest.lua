fx_version 'bodacious'
game 'gta5'

client_scripts {
    'client/Client.net.dll',
	'client/Shared.CrossCutting.net.dll',
	'client/Newtonsoft.Json.net.dll'
}

files {
	'client/MenuAPI.dll'
}
-- move ..\build\client\Newtonsoft.Json.net.dll ..\build\

server_scripts {
    'server/Server.Application.net.dll'
}