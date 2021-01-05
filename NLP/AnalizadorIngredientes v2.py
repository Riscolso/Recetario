# -*- coding: utf-8 -*-
from typing import List, Generic, Dict, Any
import nltk

def salvarEstructura(est: Generic, nom: str) -> Any:
    """Salva una estructura en un archivo plk"""
    from pickle import dump
    n = nom+'.plk'
    output = open(n, 'wb')
    dump(est, output, -1)
    output.close()
    
def cargarEstructura(fname):
    """Carga una estrutura a partir de una archivo plk"""
    from pickle import load
    n = fname+'.plk'
    f = open(n, 'rb')
    est = load(f)
    f.close()
    return est

def lematizeYPOSTAG(words: List[str]) -> List[str]:
    """Lleva todas las palabras  de una lista a su forma lematizada
    además de etiquetar el tipo de palabra"""
    from pickle import load
    f = open('LemmasTag.plk', 'rb')
    lemmas = load(f)
    f.close()
    
    wordLemma = []
    for word in words:
        if word in lemmas.keys():
            lemma = lemmas[word]
            wordLemma.append(lemma)
        else:
            wordLemma.append(word + ' n') #Si no sabe qué es, que lo ponga como noun
    return wordLemma

def obtenerDicLemasTag(fname):
    '''Tagea una palabra y después la lemmatiza'''
    f = open(fname, encoding='utf-8')
    lines = f.readlines()
    lines = [line.strip() for line in lines]
    
    lemas = {}
    for line in lines:
        if line != '': #Modificado para los saltos de línea
            words = line.split()
            words = [w.strip() for w in words]
            wordsform = words[0]
            wordsform = wordsform.replace('#', '')
            pos = words[-2][0].lower()
            clave = wordsform 
            lemas [clave] = words[-1] + ' ' + pos
    return lemas



def quitarStopWords(toks: List[str]) -> List[str]:
    """Eliminar las stopwords de una lista de palabras sin etiquetar"""
    from nltk.corpus import stopwords
    s = stopwords.words('spanish')
    palabras = [w for w in toks if not(w in s)]
    return palabras


def obtenerPalabras(archivo: str) -> List[str]:
    import nltk
    f = open(archivo, encoding='utf-8')
    lines = f.read()
    #print(lines[:100]) 
    #palabras = [p for p in lines]
    return nltk.word_tokenize(lines)

def get_context_words(word: str, words: List[str]) -> List[str]:
    """Retorna la ventana de cada palabra ingresada
     en el documento"""
    windowSize=2
    context = []
    # Words ya lematizado
    for i in range(len(words)):
        if words[i] == word:
            for j in range(i - int(windowSize / 2), i):
                if j >= 0:
                    context.append(words[j])
            try:
                for j in range(i + 1, i + (int(windowSize / 2) + 1)):
                    context.append(words[j])
            except IndexError:
                pass
    return context


def get_context_dictionary(lineas: List[List[str]]) -> Dict[str, List[str]]:
    """Obtiene un diccionario de las palabras que se encuentran en el 
    rango de la ventana, de c/u de las palabras. Separadas por renglones
    También regresa el vocabulario"""
    # Elimina los repetidos en la lista y los vuelve vocabulario
    aux = []
    for linea in lineas:
        for palabra in linea:
            aux.append(palabra)
    vocabulario = set(aux)
    # Ordena las palabras
    vocabulario = sorted(list(vocabulario))
    # print(vocabulary)
    contextos = {}
    for linea in lineas:
        #print('liena: ', linea)
        for palabra in linea:
            #print('palabra: ', palabra)
            contexto = get_context_words(palabra, linea)
            if palabra in contextos:
                contextos[palabra].extend(contexto)
            else:
                contextos[palabra] = contexto
    #print('Hay ', len(vocabulario), ' palabras de vocabulario\n', 'Y ', len(contextos), ' de contextos')
    return contextos, vocabulario

def get_vectors(context: Dict[str, List[str]], words: List[str]) -> Dict[str, List[int]]:
    """En un diccionario asigna a c/u de las palabras de words una 
    lista de frecuencias de palabras de la lista contexto"""
    vocabulary = set(words)
    vocabulary = sorted(list(vocabulary))
    llaves = context.keys()
    vectores = dict()
    for palabra in llaves:
        # Lista temporal con el contexto de cada palabra
        contexto_Palabras = context.get(palabra)
        vectorTemp = []
        for palabraEncontrada in vocabulary:
            if palabraEncontrada in contexto_Palabras:
                vectorTemp.append(contexto_Palabras.count(palabraEncontrada))
            else:
                vectorTemp.append(0)
        vectores[palabra] = vectorTemp
    return vectores

def obtenerCos(v: List[int], v2: List[int]) -> float:
    '''Obtiene el coseno del ángulo entre dos vectores'''
    import numpy as np 
    v = np.array(v)
    v2 = np.array(v2)
    return  np.dot(v,v2) / \
    ((np.sqrt(np.sum(v ** 2)))*(np.sqrt(np.sum(v2**2))))


def obtenerSimilaresVectorTag(palabra: str, vectores: Dict[str, List[int]], filtros:List[str]) -> Dict[str, float]:
    """Retorna un diccionario de clave una palabra y de valor y/o el valor
    total de la suma de los cosenos de una lista de filtros, el cual indica el nivel de similitud
    con 'palabra'
    Se toma en cuenta el tag, para hacer el cálculo o no """
    d = {}
    valor = 0
    #Comprobar que exista en los vectores
    if palabra in vectores:
        for filtro in filtros:
            filtro += ' n'
            c = round(obtenerCos(vectores[palabra],vectores[filtro]), 3)
            d[filtro] = c
            valor += c
    return d, valor

def ordenarDicXValor(d: Dict[str, float or int], bandera=True) -> Dict[str, float]:
    """Ordena un diccionario con base a su valor y a la badera
    Si la bandera es True, ordena de mayor a menor,
    Si la bandera es False, ordena de menor a mayor"""
    aux = {}
    import operator
    valOrd = sorted(d.items(), key=operator.itemgetter(1), reverse=bandera)
    
    for name in enumerate(valOrd):
        aux[name[1][0]] = d[name[1][0]]
    return aux


def get_clean_tokens(tokens: List[str]) -> List[str]:
    """Receives a list of raw tokens and returns tokens of letters only."""
     
    import re
     
    clean_tokens=[]
    for token in tokens:
        clean_token_list=[]
        for char in token: 
            if re.match(r'[a-záéíóúñü0-9]', char):#for Spanish alphabet ###MODIFIQUE
                clean_token_list.append(char)
         
        clean_token_string = ''.join(clean_token_list)
         
        if clean_token_string != '':
            clean_tokens.append(clean_token_string)
    return clean_tokens
     

def minimizarPalList(l: List[str]) -> List[str]:
    """Convierte todas las palabras de una lista de strings
    a minúsculas"""
    aux = []
    for cad in l:
        aux.append(cad.lower())
    return aux

def imprimirDicLim(d: Dict[str, float], n: int, relacion: str) -> Any:
    """Imprime los primeros n valores de un diccionario"""
    from prettytable import PrettyTable
    #imprimir tabla
    t = PrettyTable()
    t.field_names = ['Palabra', ' ',  'Valor']
    
    if d.keys == None:
        print("No hay palabras disponibles :C")
    else:
        i=0
        for key in d:
            if i<n:
                t.add_row([key, relacion, d[key]])
                i+=1
            else:
                print(t)
                break
        

def normalizar(entrada: str or List[str], archivo: bool) -> List[List[str]]:
    '''Normaliza un texto, la entrada puede ser una lista de palabras
    o el nombre de un archivo con las palabras'''
    import nltk
    if archivo:
        #Obtener lista de renglones de palabras 
        listaPalabras = obtenerListaRenglones(entrada)
    else:
        listaPalabras = [nltk.word_tokenize(entrada)]
    #Minimizar
    listaPalabras = [minimizarPalList(linea) for linea in listaPalabras]
    #Eliminar signos de puntuación
    listaPalabras = [get_clean_tokens(linea) for linea in listaPalabras]
    #print(listaPalabras[:50])
    #Eliminar stopwords
    #listaPalabras = [quitarStopWords(linea) for linea in listaPalabras]
    listaPalabras = [lematizeYPOSTAG(linea) for linea in listaPalabras]
    if not archivo:
        return listaPalabras[0]
    return listaPalabras

def obtenerListaRenglones(entrada: str) -> List[List[str]]:
    import nltk
    f = open(entrada, encoding='utf-8')
    lines = f.readlines()
    renglones = []
    for linea in lines:
        renglones.append(nltk.word_tokenize(linea))
    return renglones

if __name__ == '__main__':
    ingrediente = "2 baka mitai picado en cubos"


    '''Generar o cargar los diccionarios que se van a usar'''
    #lemmas = obtenerDicLemasTag('generate.txt')
    #salvarEstructura(lemmas, 'LemmasTag')
    lemmas = cargarEstructura('LemmasTag')

    

    #------------------ Método de fuerza bruta --------------
    #Normalizar la cadena de ingredientes de la nueva receta
    listaPalIn = normalizar(ingrediente, False)


    #Crear diccionario de corpus
    #CorpusNormalizado = normalizar('Corpus ingredientes 2.txt', True)
    #Salvar texto normalizado
    #salvarEstructura(CorpusNormalizado, 'LineasNorm')
    CorpusNormalizado = cargarEstructura('LineasNorm')
    #Agregar las palabras de los nuevos ingredientes al corpus
    CorpusNormalizado.append(listaPalIn)
    #Obtener una lista de palabras
    listaPalabras = []
    for linea in CorpusNormalizado:
        for palabra in linea:
            listaPalabras.append(palabra)

    #Obtener diccionario de contextos y el vocabulario
    dicContext, vocabulario = get_context_dictionary(CorpusNormalizado)

    #---------------- Analizar palabras -----------------
    
    #Obtener los vectores de las palabras
    vectores = get_vectors(dicContext, listaPalabras)

    # #Obtener las palabras similares de una palabra
    
    #Obtener la lista de ingredientes
    ingredientes = obtenerPalabras('Lista de ingredientes Limpios.txt')
    salvarEstructura(ingredientes, 'ingredientes')
    #ingredientes = p.cargarEstructura('ingredientes')
    print('Lista de ingredientes: ',listaPalIn,'\n')
    for palabra in listaPalIn:
        similares, valor = obtenerSimilaresVectorTag(palabra, vectores, ingredientes)
        print('La similitud de ',palabra,' es de ',valor)
        if similares != {} :
            similares = ordenarDicXValor(similares)
            imprimirDicLim(similares, 3, 'similar a')
        else:
            print('La palabra ', palabra, 'no está en la receta')