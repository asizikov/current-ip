### About

Current-IP is a little pet-project started during the COVID-19 self-isolation.


### Build and run.

Build:
`docker build -t api-test .`

Run: 

`docker run -it --rm -p 5000:80 --name api-test api-test`

And hit the url: 

`http://localhost:5000/api/currentip/latest`

and make sure to get something like that as a result: 

```json
{
    "machineName": "name",
    "currentIP": "192.168.1.1",
    "lastSeen": "2020-03-29T09:36:12.6558018+00:00"
}
```