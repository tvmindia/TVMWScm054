$(document).ready(function () {

    $('.gridTextbox').keydown(function (e) {
        try {
            if (e.which === 13) {
                var index = $('.gridTextbox').index(this) + 1;
                $('.gridTextbox').eq(index).focus();
            }
            else {

                switch (e.which) {
                    case 37: // left GridInputPerRow
                        var index = $('.gridTextbox').index(this) - 1;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 38: // up
                        var index = $('.gridTextbox').index(this) - GridInputPerRow;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 39: // right
                        var index = $('.gridTextbox').index(this) + 1;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 40: // down
                        var index = $('.gridTextbox').index(this) + GridInputPerRow;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    default: return; // exit this handler for other keys
                }

            }
        } catch (x) { }



    });

});