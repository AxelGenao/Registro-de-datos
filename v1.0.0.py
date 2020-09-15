import os
import sys

if(len(sys.argv) == 1):
    print("No se han introducido los datos")
    sys.exit()

if (os.path.exists(sys.argv[1]) == False):
    with open(sys.argv[1], "w+") as creator:
        creator.write("Cédula;Nombres;Apellidos;Edad\n")

def CedExist(x):
    with open(sys.argv[1], "r") as reader:
        if x + ";" in reader.read():
            return True

while (True):
    ced = input("\nCédula: ")
    name = input("Nombre: ")
    ape = input("Apellidos: ")
    age = input("Edad: ")
    
    if(ced == "" and name == "" and ape == "" and age == ""):
        sys.exit()
    
    if(CedExist(ced)):
        print("\nLa cédula ya existe, favor escribir la correcta!!")
    else:
        while (True):
            opt = input("\nGuardar(G), Rehacer(R), Salir(S)\n")
            
            if (opt == "G"):
                with open(sys.argv[1], "a") as writer:
                    writer.write(ced + ";" + name + ";" + ape + ";" + age + "\n")
                break
            elif(opt == "R"):
                break
            elif(opt == "S"):
                sys.exit()
            else:
                continue