$(window).load(function () {
    $("tr:even").css("background-color", "#d3d3d3");
    $("#tabs").tabs({
        heightStyle: "auto",
        start: function (ui) {
            ui.helper.css("background-color", "#BD0100").css("border-color", "#FFFFFF").css("font-color", "#FFFFFF");
        }
    });

    $("#tabs_pagamento").tabs({
        heightStyle: "auto",
        start: function (ui) {
            ui.helper.css("background-color", "rgb(238, 238, 238);").css("border-color", "#FFFFFF").css("font-color", "#FFFFFF");
        }
    });

    $("#dtNascCliente").datepicker({
        yearRange: "1900:2017",
        changeYear: true,
        changeDay: true,
        dateFormat: 'dd/mm/yy'
    });
    

    var myDate = new Date();
    var year = 2100;
    var selectData = "";

    for(var i = 1990; i < year+1; i++){
        selectData = selectData + '<option value="'+i+'">'+i+'</option>';
    }

    $("#card-year").html(selectData);
});

$("#goro").change(function () {
    var idGoro = $("#goro :selected").val();
    if (idGoro != 0) {
        $.ajax({
            type: "GET",
            data: { id: idGoro },
            url: '/Checkout/checkOutEnderecoUni',
            dataType: 'json', // <------------ OLHA O JASON 
            success: function (data) {
                var end = data.endereco;
                var comp = data.compl;
                var cep = data.cep;
                var noob = data.noob;
                var create = "<p>End: " + end + ", Nº " + noob + "</p><p>CEP: " + cep + "</p><p>Complemento: " + comp + "</p>";
                $("#panel-body").empty();
                $("#panel-body").html(create);
            },
            fail: function () {
                alert("Deu ruim");
            },
            always: function () {
                alert();
            }
        });
    } else {
        $("#panel-body").empty();
    }

});

$(".order").click(function () {
    var ordem = $(this).attr("id");
    var idBack = getUrlParameter('id');
    $.ajax({
        type: "GET",
        data: { id: idBack, order: ordem },
        url: '/Produto/ListaProdRequest',
        dataType: 'json', // <------------ OLHA O JASON AQUI Ó!
        success: function (data) {
            $("#produtos").empty();
            $("#produtos").html("<img src='../Content/Imagens/giphy.gif'");
            for (g = 0; g < data.length; g++) {
                var chtml = "";

                price = data[g].precProduto.toFixed(2);
                chtml += ' <li class="product"><a id="product-' + data[g].idProduto + '" href="~/Produto/descProduto/' + data[g].idProduto + '" data-product-id="' + data[g].idProduto + '" data-product-name="' + data[g].nomeProduto + '" data-product-price="' + price + '">';
                if (data[g].imagem64 != null && data[g].imagem64 != "0") {
                    chtml += ' <img src = "data:image/png;base64,' + data[g].imagem64 + '" id="product-image-' + data[g].idProduto + '" class="product-image" alt="' + data[g].nomeProduto + '" title="' + data[g].nomeProduto + '" />';
                } else {
                    chtml += '<img  src = "../Content/Imagens/produto-sem-imagem.gif" id="product-image-' + data[g].idProduto + '" class="product-image" alt="' + data[g].nomeProduto + '" title="' + data[g].nomeProduto + '" />';
                }
                strNome = data[g].nomeProduto;
                if (strNome.Length > 20) {
                    strNome = data[g].nomeProduto.Substring(0, 20) + "...";
                }
                chtml += '<div class="product-info"><p class="product-name">' + data[g].nomeProduto + '</p>';



                if (data[g].descontoPromocao > 0) {
                    decPrec = data[g].precProduto - data[g].descontoPromocao;
                    chtml += '  <p class="product-desc" style="color:gray; font-size:16px;">R$ ' + price + '</p><p class="product-price" style="color:black;">R$ ' + decPrec.toFixed(2) + '</p>';
                }
                else {
                    chtml += ' <br /><br /><p class="product-price">R$ ' + price + '</p>';
                }
                chtml += '<p style="margin-top: -20px;"><a href="/Produto/descProduto?id=' + data[g].idProduto + '" class="btn btn-default" style="margin-top: -2px;">Ver detalhes</a><a href="/Produto/Carrinho"type="button" class="add-cart" data-product-id="' + data[g].idProduto + '" data-product-name="' + data[g].nomeProduto + '" data-product-price="' + price + '"><span class="glyphicon glyphicon-shopping-cart"></span></a></p>';
                $("#produtos").append(chtml);
            }
        },
        fail: function () {
            alert("Falhou");
        }

    });
});

$(".mais").click(function () {
    var ordem = $(this).attr("id");
    var idBack = getUrlParameter('id');
    $.ajax({
        type: "GET",
        data: { id: idBack, at: ordem },
        url: '/Produto/ListaProdMais',
        dataType: 'json', // <------------ OLHA O JASON 
        sucess: function (data) {
            $("#produtos").html("<img src='../Content/Imagens/giphy.gif'");
            for (g = 0; g < data.length; g++) {
                var chtml = "";

                price = data[g].precProduto.toFixed(2);
                chtml += ' <li class="product"><a id="product-' + data[g].idProduto + '" href="~/Produto/descProduto/' + data[g].idProduto + '" data-product-id="' + data[g].idProduto + '" data-product-name="' + data[g].nomeProduto + '" data-product-price="' + price + '">';
                if (data[g].imagem64 != null && data[g].imagem64 != "0") {
                    chtml += ' <img src = "data:image/png;base64,' + data[g].imagem64 + '" id="product-image-' + data[g].idProduto + '" class="product-image" alt="' + data[g].nomeProduto + '" title="' + data[g].nomeProduto + '" />';
                } else {
                    chtml += '<img  src = "../Content/Imagens/produto-sem-imagem.gif" id="product-image-' + data[g].idProduto + '" class="product-image" alt="' + data[g].nomeProduto + '" title="' + data[g].nomeProduto + '" />';
                }
                strNome = data[g].nomeProduto;
                if (strNome.Length > 20) {
                    strNome = data[g].nomeProduto.Substring(0, 20) + "...";
                }
                chtml += '<div class="product-info"><p class="product-name">' + data[g].nomeProduto + '</p>';



                if (data[g].descontoPromocao > 0) {
                    decPrec = data[g].precProduto - data[g].descontoPromocao;
                    chtml += '  <p class="product-desc" style="color:gray; font-size:16px;">R$ ' + price + '</p><p class="product-price" style="color:black;">R$ ' + decPrec.toFixed(2) + '</p>';
                }
                else {
                    chtml += ' <br /><br /><p class="product-price">R$ ' + price + '</p>';
                }
                chtml += '<p style="margin-top: -20px;"><a href="/Produto/descProduto?id=' + data[g].idProduto + '" class="btn btn-default" style="margin-top: -2px;">Ver detalhes</a><a href="/Produto/Carrinho"type="button" class="add-cart" data-product-id="' + data[g].idProduto + '" data-product-name="' + data[g].nomeProduto + '" data-product-price="' + price + '"><span class="glyphicon glyphicon-shopping-cart"></span></a></p>';
                $("#produtos").append(chtml);
            }
        }

    });
});

$('#btnBoleto').click(function () {
    $(".form-metodo").attr("Action", "/Checkout/PagamentoBoleto");
    if ( $("#goro :selected").val() == 0) {
        alert("Escolha um endereço de entrega antes de finalizar o pedido!");
        return false;
    }
});

$('#btnCartao').click(function () {
    $(".form-metodo").attr("Action", "/Checkout/PagamentoCartao");
    if ($("#goro :selected").val() == 0) {
        alert("Escolha um endereço de entrega antes de finalizar o pedido!");
        return false;
    }
});

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};


