### About

Current-IP is a little pet-project started during the COVID-19 self-isolation.


### Build and run.

Local run:

`docker-compose up -d`

two containers should be brought up and the API should be seen at:

`http://127.0.0.1:8080/api/currentip/latest`

and make sure to get something like that as a result:

```json
{
    "machineName": "name",
    "currentIP": "192.168.1.1",
    "lastSeen": "2020-03-29T09:36:12.6558018+00:00"
}
```

### Build and install a Windows Service

navigate to `CurrentIp.Service` project and run

```
dotnet build

dotnet publish -r win-x64 -c Release
```

note the output, you'll see something like

```
CurrentIp.Service -> C:\work\current-ip\src\CurrentIp.Service\bin\Release\netcoreapp3.1\win-x64\publish\
```

you need a full path, copy it and run (as an Administator):

```
sc create CurrentIp.Service BinPath=C:\work\current-ip\src\CurrentIp.Service\bin\Release\netcoreapp3.1\win-x64\publish\CurrentIp.Service.exe
sc start CurrentIp.Service
```

and you've got it running:

![docs/win-service-running.pn](docs/win-service-running.png)

