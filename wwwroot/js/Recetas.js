

function AgregarPaso() {
    //alert("actualizado");
    var paso = p;
    p++;
    //Traer el div de pasos
    var divPasos = document.getElementById("pasos")
    //Crear el div que tiene un paso
    var div = document.createElement("div");
    div.id = 'paso_'+paso;
    //Dar clase a el div
    div.className = "form - group";
    //Crear los inputs
    var lb = document.createElement("label");
    lb.className = "control-label";
    lb.textContent = "Paso";

    var inNoPaso = document.createElement("input");
    inNoPaso.type = "text";
    //inNoPaso.value = p;
    inNoPaso.value = paso;
    
    inNoPaso.name = "Pasos[" + paso + "].NoPaso";
    inNoPaso.class = "form-control";
    //inNoPaso.formAction = "/Usuarios/Recetas/Crear";
    //inNoPaso.formMethod = "post";

    var inTexto = document.createElement("input");
    inTexto.type = "text";
    inTexto.value = "";
    inTexto.name = "Pasos[" + paso + "].Texto";

    var inTiempo = document.createElement("input");
    inTiempo.type = "time";
    inTiempo.defaultValue = "";
    inTiempo.name = "Pasos[" + paso + "].TiempoTemporizador";

    var img = document.createElement("input");
    img.type = "file";
    img.name = "Pasos[" + paso + "].Imagen";

    var btnQuitar = document.createElement("button");
    btnQuitar.type = "button";
    //Agregamos la función del eliminar el propio div
    btnQuitar.onclick = function eliminarElemento() {
        elem = document.getElementById('paso_' + paso);
        if (!elem) {
            alert("El paso no existe");
        } else {
            padre = elem.parentNode;
            padre.removeChild(elem);
        }
    }
    btnQuitar.innerHTML = "-";


    //Agregamos los elementos al div
    div.appendChild(lb);
    div.appendChild(inNoPaso);
    div.appendChild(inTexto);
    div.appendChild(inTiempo);
    div.appendChild(btnQuitar);
    div.appendChild(img);
    //Agregamos el div al div de los pasos
    divPasos.appendChild(div);
}

//No sé por que no funciona si lo llamo al momento de crear botones jajajaja
//Pero lo necesito aquí afuera para llamarlo al editar los pasos
function QuitarElemento(id) {
    elem = document.getElementById(id);
    if (!elem) {
        alert("El paso no existe"+id);
    } else {
        padre = elem.parentNode;
        padre.removeChild(elem);
    }
}