(function () {
    //change order price after remove product from cart
    const chagePrice = function (decrSum) {
        let span = $("#sumSpan");
        span.text((+(span.text().replace(",", ".")) - decrSum).toFixed(2));
    }

    //delete product from cart
    const deteleFromCart = function (productId, JQview) {
        $.ajax({
            url: '/Cart/Delete/', 
            data: {id: productId},
            type: 'POST',
        }).done(function (data) {
            showMsg(data.Massage);
            chagePrice(Number.parseFloat(JQview.find(".product_price").text().replace(",", "."), 10));
            JQview.remove();
        }).fail(function (data) {
            if (data.Massage) showMsg(data.Massage);
            else showMsg("Помилка видалення!")
        });
    }

    //show delete confirm window
    const deleteConfirm = function(jqelement) {
        let productId = jqelement.attr("delete");
        let productView = jqelement.closest(".product-element");
        let productName = jqelement.closest(".card").find(".good-name").text();
        let msg = "Ви бажаєте видалити \"" + productName + "\" з корзини?"

        confirmMsg(msg, () => deteleFromCart(productId, productView));
    }

    //set events
    $("[delete]").on("click", function () {
        let elemet = $(this);
        checkAccount(() => deleteConfirm(elemet));
    })
    
})()