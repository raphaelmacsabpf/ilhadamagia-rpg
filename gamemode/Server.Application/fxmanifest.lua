fx_version 'bodacious'
game 'gta5'

client_scripts {
    'client/Client.Application.net.dll',
	'client/Shared.CrossCutting.net.dll',
	'client/Newtonsoft.Json.net.dll'
}
ui_page "client/nui/html/index.html"

files {
	'client/MenuAPI.dll',
	'client/LZ4.dll',
	'client/LZ4pn.dll',
	'client/nui/html/96c342ee6d5d50fc5b3fa76a6f6932b9.tff',
	'client/nui/html/index.html',
	'client/nui/html/main.js'
}
-- move ..\build\client\Newtonsoft.Json.net.dll ..\build\

server_scripts {
    'server/Server.Application.net.dll'
}