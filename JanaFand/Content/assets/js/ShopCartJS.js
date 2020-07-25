
/*===================================*
00. ShowCart JS
/*===================================*/

function CommandFactor(id, count) {
    $.ajax({
        url: "/ShopCart/CommandFactor/" + id,
        type: "Get",
        data: { count: count }
    }).done(function (res) {
        $("#ShowFactor").html(res);
    });
}

$(function () {
    countShopCart();
})


function countShopCart() {

    $.get("/Api/Shop", function (res) {
        $("#countShopCart").html(res);
    });

}


function AddToCart(id) {
    $.get("/Api/Shop/" + id, function (res) {
        $("#countShopCart").html(res);
        updateShowCart();

    });
}

function updateShowCart() {
    $("#ShowCart").load("/ShopCart/ShowCart", "#ShowCart");


}

	/*===================================*
	00. End ShowCart JS 
	/*===================================*/