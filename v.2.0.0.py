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
    menu = input("\n1. Capturar\n2. Listar\n3. Búsqueda\n4. Salir\n")
    
    if(menu == "1"):
        while(True):
            ced = input("\nCédula: ")
            name = input("Nombre: ")
            ape = input("Apellidos: ")
            age = input("Edad: ")
            
            if(ced == "" and name == "" and ape == "" and age == ""):
                break
            
            if(CedExist(ced)):
                print("\nLa cédula ya existe, favor escribir la correcta!!")
            else:
                while (True):
                    opt = input("\nGuardar(G), Rehacer(R), Salir(S)\n")
                    
                    if (opt == "G"):
                        with open(sys.argv[1], "a") as writer:
                            writer.writelines(ced + ";" + name + ";" + ape + ";" + age)
                        break
                    elif(opt == "R"):
                        break
                    elif(opt == "S"):
                        sys.exit()
                    else:
                        continue
    elif (menu == "2"):
        print()
        with open(sys.argv[1], "r") as reader:
            data = reader.readlines()
            for i in data:
                dat = i.replace("\n", "")
                print(dat)
    elif (menu == "3"):
        searchCed = input("\nIntroduzca la cédula que desea buscar: ")
        
        if(CedExist(searchCed)):
            with open(sys.argv[1], "r") as reader:
                data = reader.readlines()
                for i in data:
                    if searchCed + ";" in i:
                        print(i)
        else:
            print("\nLa cédula introducida no existe...")
    elif (menu == "4"):
        sys.exit()
    else:
        print("La opción introducida no es aceptable!!")