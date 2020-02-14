import os
from urllib.request import urlopen
import gzip
import shutil
from itertools import islice
import xml.etree.cElementTree as ET
import datetime
import time
from elasticsearch import Elasticsearch
from elasticsearch import helpers

file_url = 'https://dumps.wikimedia.org/plwiki/latest/plwiki-latest-abstract.xml.gz'
file_name = "plwiki-latest-abstract.xml"
es_host = '192.168.1.107'
es_port = 9200
es_index_name = 'wiki_abstract_pl'

start_time = time.time()

archive_name = file_url.split('/')[-1]

outfile = open(archive_name, "wb")
outfile.write(urlopen(file_url).read())
outfile.close()
with gzip.open(archive_name, 'rb') as f_in:
    with open(file_name, 'wb') as f_out:
        shutil.copyfileobj(f_in, f_out)

elapsed_time = time.time() - start_time
print("download took: " + str(round(elapsed_time)) + " seconds")

root = ET.parse(file_name).getroot()

datas = []
for doc in list(root.findall('doc')):
    title = doc.find('title').text
    content = doc.find('abstract').text
    if (content == None):
        continue
    if (len(content.replace(" ", "")) > 40):
        data = {}
        try:
          data['Title'] = title.replace("Wikipedia: ", "")
        except:
          continue
        data['Content'] = content
        datas.append(data)

root.clear()
del root

es = Elasticsearch(hosts = [{'host': es_host, 'port': es_port}])

try:
    es.indices.delete(index = es_index_name)
except:
    print("Index not created or already deleted")

start_time = time.time()

actions = [
    {
        "_index": es_index_name,
        "_source": {
            "title": data['Title'],
            "content": data['Content'],
            "timestamp": datetime.datetime.now()}
    }
    for data in datas
]

elapsed_time = time.time() - start_time
print("adding documents took: " + str(round(elapsed_time)) + " seconds")

helpers.bulk(es, actions)

print("done")