function Salvar() {
    $("#p").html("<p></p>");
    var local, quantidade, transporte, tipoPasseio, dataPasseio;
    var erro = "<ul>";

    if ($("#escolhaLocal").val() == "0") {
        erro += "<li>Preencha o Local</li>";
    } else
        local = $("#escolhaLocal").val();


    if ($("#qtdPessoas").val() == "0") {
        erro += "<li>Preencha a quantidade de pessoas</li>";
    } else
        quantidade = $("#qtdPessoas").val();


    if ($("#tipoPasseio").val() == "0") {
        erro += "<li>Selecione o tipo passeio</li>";

    } else
        transporte = $("#tipoPasseio").val();


    if ($("#transporte").val() == "0") {
        erro += "<li>Selecione o transporte</li>";
    } else
        tipoPasseio = $("#transporte").val();


    if ($("#data").val() == "0") {
        erro += "<li>Preencha a data de passeio</li>";
    } else
        dataPasseio = $("#data").val();

    erro += "</ul>"

    if (erro == "<ul></ul>") {

        var jsonText = JSON.stringify({ local: local, quantidade: quantidade, transporte: transporte, tipoPasseio: tipoPasseio, dataPasseio: dataPasseio });

        $.ajax({
            type: 'POST',
            url: 'indexContent.aspx/Salvar',
            data: jsonText,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                if (r.d === "") {
                    alert("não trouxe nada do bd");
                } else {
                    //alert("Avaliação cadastrada com sucesso!");
                    //$("#lblCards").html(r.d);
                    $("#iti_cards").html(r.d);
                    $("#itinerarios").fadeIn();
                    $("#mapContainer").css({ "display": "none" });
                }
            }
        });

    } else {
        $(".msg").append(erro);
    }
    $("#teste").val() = local;
}

function EscolherGuia(pt_iti_cod) {

    var jsonText = JSON.stringify({ pt_iti_cod: pt_iti_cod });

    $.ajax({
        type: 'POST',
        url: 'indexContent.aspx/EscolherGuia',
        data: jsonText,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            if (r.d === "") {
                alert("ERROR 404");
            } else {

                $("#guias_cards").html(r.d);

                $("#guias").fadeIn();
                $("#itinerarios").css({ "display": "none" });
            }
        }
    });
}