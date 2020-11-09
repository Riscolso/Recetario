var p = 1;

function AgregarPaso() {
    var paso = p;
    alert("aaaaa")
    //Traer el div de pasos
    var divPasos = document.getElementById("pasos")
    //Crear el div que tiene un paso
    var div = document.createElement("div");
    //Dar clase a el div
    div.className = "form - group";
    //Crear los inputs
    var lb = document.createElement("label");
    lb.className = "control-label";
    lb.textContent = "Paso";

    var inNoPaso = document.createElement("input");
    inNoPaso.type = "text";
    //inNoPaso.value = p;
    inNoPaso.value = "";
    p++;
    inNoPaso.name = "Pasos[" + paso + "].NoPaso";
    inNoPaso.class = "form-control";
    inNoPaso.formAction = "/Usuarios/Recetas/Crear";
    inNoPaso.formMethod = "post";

    var inTexto = document.createElement("input");
    inTexto.type = "text";
    inTexto.value = "";
    inTexto.name = "Pasos[" + paso + "].Texto";

    var inTiempo = document.createElement("input");
    inTiempo.type = "time";
    inTiempo.defaultValue = "";
    inTiempo.name = "Pasos[" + paso + "].TiempoTemporizador";

    //Agregamos los elementos al div
    div.appendChild(lb);
    div.appendChild(inNoPaso);
    div.appendChild(inTexto);
    div.appendChild(inTiempo);
    //Agregamos el div al div de los pasos
    divPasos.appendChild(div);
}