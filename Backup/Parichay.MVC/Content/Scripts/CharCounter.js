var CharCounter =
{
    init: function (options) {
        this.CharCounter(options);
    },
    mini: function (options) {
        var defaults = { maxCount: 255, warnCount: 220, denyCount: 240, lblCharCountClass: "p.lblCharCount" };
        defaults = $.extend(defaults, options);
        this.CharCounter(defaults);
    },
    CharCounter: function (options) {
        /* attach a submit handler to the form */
        $(".charCounter").keyup(function () {

            //Default
            var defaults = { maxCount: 2000, warnCount: 1900, denyCount: 1950, lblCharCountClass: "p.lblCharCount" };
            defaults = $.extend(defaults, options);

            

            var value = $(this).val()
            var $fieldCounter = $(this).parent().find(defaults.lblCharCountClass);

            var formcontent = value.split(" ")
            var wordcount = formcontent.length

            // var fieldCounter = $(spanNm);
            var tmp = defaults.maxCount - value.length;

            $fieldCounter.html('Total Words:<b>'+wordcount+'</b>, Characters Remaining:<b>' + tmp + '</b>');
            if (value.length > defaults.denyCount) {
                $fieldCounter.css("color", "#d40d12");
            } else if (value.length > defaults.warnCount) {
                $fieldCounter.css("color", "#5c0002");
            } else {
                $fieldCounter.css("color", "#666666");
            }
        });

    }

};