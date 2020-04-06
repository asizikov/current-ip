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