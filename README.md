# wikipedia-articles-with-elasticsearch
Search article in Wikipedia abstract simple model

# Requriments
Computer or server with Linux Ubuntu based system (or another, but those steps could be different). Remember that the machine must meet the requirements to run the ElasticSearch server. I worked on the old Lenovo ThinkPad T410, 8GB of RAM, 2 core processor, SSD.

# How to run
1.	Install java using this tutorial: https://computingforgeeks.com/install-oracle-java-13-on-ubuntu-debian/ . Remember that to be able to download jdk, you must create an account on Oracle. so it's best to download jdk on your machine and upload to the server using for example WicSCP.
2.	Install elastic using this tutorial: https://tecadmin.net/setup-elasticsearch-on-ubuntu/
3.	Install dotnet core using this tutorial: https://docs.microsoft.com/pl-pl/dotnet/core/install/linux-package-manager-ubuntu-1904
4.	Install python using this tutorial: https://docs.python-guide.org/starting/install3/linux/
5.	Configure elastic. Type
`sudo nano /etc/elasticsearch/elasticsearch.yml`
Then modify those settings:
```
node.name: master1 #sets the node name, enter a friendly one
path.data: /var/lib/elasticsearch #data path
path.logs: /var/log/elasticsearch #log path, it can be useful in case of errors
network.host: ["_site_", "127.0.0.1"] #at which addresses it returns data, here: all interfaces + localhost
discovery.seed_hosts: 127.0.0.1 #what addresses is it looking for nodes here: only locally
```
6. Launch Elasticsearch
`sudo /bin/systemctl enable elasticsearch.service`
7.	Check that the server is working properly.
on the server:
`curl  http://localhost:9200/_cluster/health?pretty`
on remote, eg. From PostMan
`GET http://serverIp:9200/_cluster/health?pretty`
8.	The data will come from the simplified Wikipedia dataset in Polish language. The data is available at address https://dumps.wikimedia.org/plwiki/latest/ and the newest - at address https://dumps.wikimedia.org/plwiki/latest/plwiki-latest-abstract.xml.gz . You can also find datasets for other languages, try to modify the url or search similiar phrases in google.
9.	Install missing python packages.
`pip install cElementTree`
`pip install elasticsearch`
10.	 Clone this repo, go to 
`cd wikipedia-articles-with-elasticsearch/WikipediaSearchEngine/WikipediaSearchEngine`
then type
`nano appsettings.json`
and make sure, that endpoint to elasticsearch is proper for your machine.
11.	 Now run website. To do this, type:
`dotnet run`
12.	That’s all. Now go to browser and type server adress (or localhost on this machine) with proper port and press red button on navbar to recreate index.

# How to use

### Home page view
![01_Home.png](Images/01_Home.png)

### The live search refreshes the results in the list
![02_Searching.png](Images/02_Searching.png)

### The application allows you to test its API, which is used by JS scripts.
![03_TestApi.png](Images/03_TestApi.png)

### You can regenerate the index at any time. The wikipedia file is updated every few weeks and is located at the same address that is given in the script.
![04_IndexRecreating.png](Images/04_IndexRecreating.png)

### Application blocks the ability to generate an index when it is currently being generated
![05_ErrorIndexAlreadyRecreating.png](Images/05_ErrorIndexAlreadyRecreating.png)

# Python script documentation
Just go to `Docs` folder and open `Parse data & load into engine.ipynb`