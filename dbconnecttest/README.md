# DatabaseConnectionTest
Generic headless dotnet core app that can be easily rebranded for demos.  Connects to an SQL instance, executes queries on a periodic interval and logs the results.

This is a fork from: https://github.com/twright-msft/DatabaseConnectionTest/tree/master

To build it locally, run it from the DatabaseConnecitonTest: `docker build -t test -f .\DatabaseConnectionTest\Dockerfile .`

## To Build
- Just click the DatabaseConnectionTest button in the Build toolbar

## To Publish
- Right click on the DatabaseConnectionTest project in the Solution Explorer in the tree and choose Publish...
- Verify the configuration settings, especially which registry the container image will be published to
- Click Publish

## To Deploy
- Change the registry, repository, and image tag as needed in the /GitOps/yaml/app-deployment.yaml file
- Change the storage class and service type as needed in the app-deployment.yaml file
- Run `kubectl create -f <path to app-deployment.yaml file> -n <namespace>`
- Wait for the pod to come up completely
- To see the logs execute the command `kubectl logs dbconnecttest <pod name> -n <namespace> --follow
- To redploy first run `kubectl delete -f <path to app-deployment.yaml file> -n <namespace>` and then `kubectl create -f <path to app-deployment.yaml file> -n <namespace>` again.

## To Rebrand:

Generally, you can search through the entire solution for "DEMO_CUSTOMIZATON" to find all the places where the code can be customized.

### Rebrand container registry
You will need to create your own container registry where you can push your modified images to and then you will need to change a few things as follows to publish to your registry and then deploy from there:
- Create a container image repository someplace like Azure Container Registry (ACR)
- Change the container registry in the GitOps/yaml/app-deployment.yaml file.

		```
		image: databaseconnectiontest.azurecr.io/databaseconnectiontest:<tag>
		```

- Change the Publish profile configuration to also point to the container registry
- Publish to make sure that end:end building and publishing to the container registry works