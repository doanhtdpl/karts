-- 1. For all the Objects in the scene
-- 1.1	If it is no saved we save it
-- 1.2  else if the Object is an instance we do nothing
-- 1.3 We save the position, rotation, extra properties and object names in an xml


-- Retorna el buffer de las user properties
getUserPropBuffer($)

-- Retorna el numero de instancias de un objeto seleccionado
InstanceMgr.GetInstances $ &instances

-- Retorna una lista de los objetos originaes (no instanciados) en la escena
-- Types: Editable_Mesh, Box, Sphere, Editable_Poly
list = getClassInstances(Box)

for e in list do 
(
	-- Devuelve una lista con los objetos instanciados
	instances = refs.dependents e
	
	for i in instances do
		print(obj.name)
)

--ExportFile ("C:\\temp\\max2fbxExported\\" + getFilenameFile (MaxFileName) + ".fbx") [ #noPrompt ] ;

node = undefined

-- We make sure the selected object is a root box!
if (selection[1] != undefined) then
	if (selection[1].parent == undefined and selection[1].name == "Root") then
		node = selection[1]
	else if (selection[1].parent != undefined and selection[1].parent.name == "Root" and findString selection[1].name "SubNode" != undefined) then
		node = selection[1]


-- The circuit will be divided in N parts for drawing optimizations issues
-- If The root node is selected this means that we will export all the circuits parts
-- However if we selected a subnode we will only export that circuit part (think a bit about this)

children = 
for e in l do ( print(e.name) )


