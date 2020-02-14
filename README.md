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
6.	Check that the server is working properly.
on the server:
`curl  http://localhost:9200/_cluster/health?pretty`
on remote, eg. From PostMan
`GET http://serverIp:9200/_cluster/health?pretty`
7.	The data will come from the simplified Wikipedia corpus in Polish. The data is available at address https://dumps.wikimedia.org/plwiki/latest/ and the newest - at address https://dumps.wikimedia.org/plwiki/latest/plwiki-latest-abstract.xml.gz  . You can also find bodies for other languages, try to modify the url or search in google.
8.	Install missing python packages.
`pip install cElementTree`
`pip install elasticsearch`
9.	 Clone this repo, go to 
`cd wikipedia-articles-with-elasticsearch\WikipediaSearchEngine\WikipediaSearchEngine`
then type
`nano appsettings.json`
and make sure, that endpoint to elasticsearch is proper for your machine.
10.	 Now run website. To do this, type:
`dotnet run`
11.	That’s all. Now go to browser and type server adress (or localhost on this machine) with proper port and press red button on navbar to recreate index.
