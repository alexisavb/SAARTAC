import dicom
import sys, os

DimensionesPixel = 1
Edad = 3
EspesorRebanada = 6
Fecha = 5
Hospital = 7
MatrizDICOM = 0
Nombre = 2
Sexo = 4

def LeerArchivosDICOM(ruta):
	return dicom.read_file(ruta)

pregunta = int(sys.argv[1]) 
ruta = sys.argv[2]
archivoDicom = LeerArchivosDICOM(ruta)
if pregunta == MatrizDICOM:	
	print len(archivoDicom.pixel_array), len(archivoDicom.pixel_array[0])
	for j in archivoDicom.pixel_array:
		for k in xrange(len(j) - 1):
			print j[k],
		print j[-1]
elif pregunta == DimensionesPixel:
	print archivoDicom[0x28, 0x30].value[0], archivoDicom[0x28, 0x30].value[1]
elif pregunta == Nombre:
	print archivoDicom[0x10, 0x10].value
elif pregunta == Edad:
	print archivoDicom[0x10, 0x1010].value
elif pregunta == Sexo:
	print archivoDicom[0x10, 0x40].value
elif pregunta == Fecha:
	print archivoDicom[0x8, 0x20].value
elif pregunta == EspesorRebanada:
	print archivoDicom[0x18, 0x50].value
elif pregunta == Hospital:
	print archivoDicom[0x08, 0x80].value

