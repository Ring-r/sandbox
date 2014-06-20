import os
from xml.dom.minidom import parse

#current_directory = os.path.abspath(os.curdir)
current_work_directory = os.getcwd()

class Project:
    u'Project information'

projects = []
for d, dirs, files in os.walk(current_work_directory):
    for file in files:
        if(file.endswith('.csproj')):
            project = Project()
            project.path = os.path.join(d, file)
            projects.append(project)

for project in projects:
    document = parse(project.path)
    assemblyNameListNode = document.getElementsByTagName("AssemblyName")
    project.assemblyName = assemblyNameListNode[0].firstChild.data

for project in projects:
    document = parse(project.path)
    project.references = []
    for reference in document.getElementsByTagName("Reference"):
        project.references.append(reference.getAttribute("Include").split(",", 1)[0])
    for reference in document.getElementsByTagName("ProjectReference"):
        project.references.append(reference.getElementsByTagName("Name")[0].firstChild.data)
    project.references.sort()

projects.sort(key = lambda x: x.assemblyName);
for project in projects:
    print(project.assemblyName)
    #print(project.path)
    #for reference in project.references:
    #    print(reference)
    #print('\n')
