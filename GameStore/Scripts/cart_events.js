(function () {

    const deteleFromCart = function (productId, JQview) {
        $.ajax({
            url: '/Cart/Delete/', 
            data: {id: productId},
            type: 'POST',
        }).done(function (data) {
            showMsg(data.Massage);
            JQview.remove();
        }).fail(function (data) {
            if (data.Massage) showMsg(data.Massage);
            else showMsg("Помилка видалення!")
        });
    }

    const deleteConfirm = function(jqelement) {
        let productId = jqelement.attr("delete");
        let productView = jqelement.closest(".product-element");
        let productName = jqelement.closest(".card").find(".good-name").text();
        let msg = "Ви бажаєте видалити \"" + productName + "\" з корзини?"

        confirmMsg(msg, () => deteleFromCart(productId, productView));
    }

    $("[delete]").on("click", function () {
        let elemet = $(this);
        checkAccount(() => deleteConfirm(elemet));
    })
    
})()