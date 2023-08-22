(function () {
    "use strict"

    //change text on pagination panel
    const paginationTextControll = function() {
        let skipToFirst = $(".PagedList-skipToFirst a"),
            skipToPrevious = $(".PagedList-skipToPrevious a"),
            skipToNext = $(".PagedList-skipToNext a"),
            skipToLast = $(".PagedList-skipToLast a");


        let skipToFirstText = skipToFirst.text(),
            skipToPreviousText = skipToPrevious.text(),
            skipToNextText = skipToNext.text(),
            skipToLastext = skipToLast.text();

        let windowEl = $(window);
        let PrevWindowSize = 1000;
        
        return function () {
            if (windowEl.width() <= 768 && PrevWindowSize > 768)
            {
                PrevWindowSize = windowEl.width();
                skipToFirst.text("<<");
                skipToPrevious.text("<");
                skipToNext.text(">");
                skipToLast.text(">>");
            }

            if (windowEl.width() > 768 && PrevWindowSize <= 768) {
                PrevWindowSize = windowEl.width();
                skipToFirst.text(skipToFirstText);
                skipToPrevious.text(skipToPreviousText);
                skipToNext.text(skipToNextText);
                skipToLast.text(skipToLastext);
            }
        }
    }

    $(document).ready(function () {
        $(window).on("resize", paginationTextControll());
    });
    paginationTextControll()();

})();