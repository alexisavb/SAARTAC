import dicom
import sys, os

def Leer_Archivos_DICOM(ruta):
	return dicom.read_file(ruta)

pregunta = int(sys.argv[1]) 
ruta = sys.argv[2]
archivoDicom = Leer_Archivos_DICOM(ruta)
if pregunta == 0:	
	print len(archivoDicom.pixel_array), len(archivoDicom.pixel_array[0])
	for j in archivoDicom.pixel_array:
		for k in xrange(len(j) - 1):
			print j[k],
		print j[-1]
elif pregunta == 1:
	print archivoDicom[0x28, 0x30].value[0], archivoDicom[0x28, 0x30].value[1]
