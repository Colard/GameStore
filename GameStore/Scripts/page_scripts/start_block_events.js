(function () {
    $(document).ready(function () {
        //add link to div-block
        $('[div-href]').click(function (e) {
            e.stopPropagation();
            let target = $(e.target);
            let targetHref = target.attr('href');

            if (typeof targetHref === "undefined") {
                location.href = $(this).attr("div-href");
            } 

            return true;
        });
    });
})();