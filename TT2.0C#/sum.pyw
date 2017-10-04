import dicom
import sys, os

def Leer_Archivos_DICOM(ruta):
	return dicom.read_file(ruta)

pregunta = int(sys.argv[1]) 
ruta = sys.argv[2]

if pregunta == 0:
	archivoDicom = Leer_Archivos_DICOM(ruta)
	
	print len(archivoDicom.pixel_array)
	for j in archivoDicom.pixel_array:
		for k in xrange(len(j) - 1):
			print j[k],
		print j[-1]
