cd /workspaces
git clone https://github.com/microsoft/azure-arc-jumpstart-apps.git

cd /workspaces/azure-arc-jumpstart-apps/hello-arc-windows/src

# Build Windows Server container image with Node installed, needed as Node does not have an official Windows image per 2023-02-08: https://github.com/nodejs/docker-node/pull/362
docker build ./node -t azurearcjumpstart.azurecr.io/node-windows-servercore:ltsc2019

docker push azurearcjumpstart.azurecr.io/node-windows-servercore:ltsc2019

# Build node application
docker build ./app -t azurearcjumpstart.azurecr.io/hello-arc-windows:latest

docker push azurearcjumpstart.azurecr.io/hello-arc-windows:latest

# Test locally
docker run -d -p 8080:8080 azurearcjumpstart.azurecr.io/hello-arc-windows:latest

# AKS
az login
az aks get-credentials --resource-group aks-demo-rg --name aks-demo

# Test Linux image on AKS Linux node
kubectl run hello-arc-linux --image liorkamrat/hello-arc --overrides='{"apiVersion": "v1", "spec": {"nodeSelector": { "kubernetes.io/os": "linux" }}}'

# Verify app is working
kubectl port-forward hello-arc-linux 8080:8080

# Test Windows image on AKS Windows node
kubectl run hello-arc-windows --image azurearcjumpstart.azurecr.io/hello-arc-windows:latest --overrides='{"apiVersion": "v1", "spec": {"nodeSelector": { "kubernetes.io/os": "windows" }}}'

# Verify app is working
kubectl port-forward hello-arc-windows 8080:8080

# Test Helm chart
cd /workspaces/azure-arc-jumpstart-apps
helm install --create-namespace --namespace hello-arc hello-arc-windows ./hello-arc-windows/charts/hello-arc

kubectl get deployments --namespace hello-arc

helm delete hello-arc-windows --namespace hello-arc