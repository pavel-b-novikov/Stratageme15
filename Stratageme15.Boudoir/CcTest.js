function onPopupShowWrap() {
    var wrapperHeight = $(".ui-dialog:visible").height() + $(window).scrollTop();
    var windowHeight1 = document.body.clientHeight + $(window).scrollTop();
    var windowHeight2 = document.body.clientHeight;
    $(".popup_wrapper").attr("rel", $(window).scrollTop());
    if ($(".ui-dialog:visible").height() > windowHeight2) {
        var topOffset = parseInt((wrapperHeight - windowHeight1) / 2) + $(window).scrollTop();
        $(".popup_wrapper_offset").css({ 'top': -1 * $(window).scrollTop() });
        $(".popup_wrapper").height($(".ui-dialog:visible").height()).css({ 'overflow': 'hidden' });
        $(".ui-dialog:visible").css({ 'top': 0 });
    } else {
        // Если высота попапа меньше видимой области
        var topOffset = parseInt((windowHeight2 - $(".ui-dialog:visible").height()) / 2);
        $(".popup_wrapper_offset").css({ 'top': -1 * $(window).scrollTop() });
        $(".popup_wrapper").height(windowHeight2).css({ 'overflow': 'hidden' });
        $(".ui-dialog:visible").css({ 'top': topOffset });
        setTimeout(function () {
            $(".ui-dialog").addClass('ui-dialog_ie');
        }, 50);
    }
}

function onPopupCloseDewrap() {
    $(".popup_wrapper").removeAttr("style").css({ 'overflow': 'visible' });
    $(".popup_wrapper_offset").css({ 'top': '0' });
    $(window).scrollTop($(".popup_wrapper").attr("rel"));
    $(".ui-dialog").removeClass('ui-dialog_ie');
}

function setupLabel() {
    if ($('.label_check input').length) {
        $('.label_check').each(function () {
            var curr = $(this);
            curr.removeClass('c_on');
            curr.find('input').removeClass("checked1");
            if (curr.find("input").attr("disabled")) {
                curr.css({ 'opacity': '0.5', 'cursor': 'default' });
            }
        });
        $('.label_check input:checked').each(function () {
            $(this).parent('label').addClass('c_on');
            $(this).addClass('checked1');
        });
    };
    if ($('.label_radio input').length) {
        $('.label_radio').each(function () {
            var curr = $(this);
            curr.removeClass('r_on');
            if (curr.find("input").attr("disabled")) {
                curr.css({ 'opacity': '0.5', 'cursor': 'default' });
            }
        });
        $('.label_radio input:checked').each(function () {
            $(this).parent('label').addClass('r_on');
        });
    };
};

function dialogsClose() {
    //$(".ui-widget-overlay").on("click", function() {
    //    $("#ind_settings1").dialog("close");
    //    $("#meeting_order").dialog("close");
    //    $("#auth").dialog("close");
    //    $("#logout").dialog("close");
    //});
}

function customSelectAdjust(select) {
    // Get objects
    var wrap = select.find(".c_name");
    var list = select.find("ul");
    // Let list width to autosize himself
    list.css({ 'width': 'auto' });
    // Get objects widths
    var wrapWidth = wrap.width() + 34;
    var listWidth = list.width();
    // Resize elements
    if (wrapWidth > listWidth) {
        list.width(wrapWidth);
    } else {
        wrap.width(listWidth - 34);
    }
}

function customSelectAdjustAll() {
    $(".c_select1:visible").each(function () {
        var select = $(this);
        if (!select.hasClass("transformed")) {
            var wrap = select.find(".c_name");
            var list = select.find("ul");
            var wrapWidth = wrap.width() + 34;
            var listWidth = list.width();
            if (wrapWidth > listWidth) {
                list.width(wrapWidth);
            } else {
                wrap.width(listWidth - 34);
            }
            select.addClass("transformed");
        }
    });
}

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    _title: function (title) {
        if (!this.options.title) {
            title.html("&#160;");
        } else {
            title.html(this.options.title);
        }
    }
}));
$(document).ready(function () {

    $(".label_check, .label_radio").live('click', function () {
        setupLabel();
    });
    setupLabel();

    // Float notification interactions
    $(".notify_stack_wrap .close").click(function () {
        $(".notify_stack_wrap").fadeOut(100, function () {
            $(".notify_tab").removeClass("expanded");
        });
    });
    $(".notify_tab").click(function () {
        $(".notify_stack_wrap").fadeIn(100, function () {
            $("#scrollbar_notify").mCustomScrollbar("update");
            $(".notify_tab").addClass("expanded");
        });
    });

    $("#scrollbar_notify").mCustomScrollbar({
        mouseWheel: true,
        mouseWheelPixels: "auto",
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $("#scrollbar_instruction").mCustomScrollbar({
        mouseWheel: true,
        mouseWheelPixels: "auto",
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $("#scrollbar_settings1").mCustomScrollbar({
        mouseWheel: true,
        mouseWheelPixels: "auto",
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $("#scrollbar_diagram_legend").mCustomScrollbar({
        mouseWheel: true,
        mouseWheelPixels: "auto",
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $(".scrollbar_indexes_settings").mCustomScrollbar({
        mouseWheel: true,
        mouseWheelPixels: "auto",
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $("#all_accounts_scroll").mCustomScrollbar({
        mouseWheel: true,
        advanced: {
            updateOnContentResize: true
        },
        scrollInertia: 0,
        scrollButtons: { enable: false }
    });

    $("#tabs_quotes").tabs();
    $("#tabs_consolidate").tabs();
    $("#tabs_account").tabs();
    $("#tabs_profit_dynamics").tabs();

    // All accounts dropdown menu
    $(".btn_all_accounts").click(function () {
        if ($(".all_accounts_popup").hasClass("expanded")) {
            $(".all_accounts_popup").css({ 'left': '-9000px', 'display': 'block' });
            $(".all_accounts_popup").removeClass("expanded");
        } else {
            var listHeight = $(".all_accounts_popup .accounts_list").height();
            var scrollHeight = $("#all_accounts_scroll").height();
            if (listHeight < scrollHeight) {
                $("#all_accounts_scroll").height(listHeight);
            }
            $(".all_accounts_popup").css({ 'left': '3px', 'display': 'none' });
            $(".all_accounts_popup").fadeIn(100, function () {
                $(this).addClass("expanded");
            });
        }
    });
    $("body").click(function (e) {
        if (!(e.target.className == "btn_all_accounts")) {
            if ($(e.target).closest(".all_accounts_popup.expanded").length == 0) {
                $(".all_accounts_popup").fadeOut(100, function () {
                    $(this).css({ 'left': '-9000px', 'display': 'block' });
                    $(this).removeClass("expanded");
                });
            }
        }
    });

    // Timeline settings dropdown menu
    $(".timeline_settings_lnk").click(function () {
        $(".tl_dialog_settings").fadeToggle(100);
    });
    $("body").click(function (e) {
        if (!(e.target.className == "timeline_settings_lnk")) {
            if ($(e.target).closest(".tl_dialog_settings").length == 0) $(".tl_dialog_settings").fadeOut(100);
        }
    });

    // Accounts compare dropdown menu
    $(".account_compare_lnk").click(function () {
        $(".account_compare").fadeToggle(100);
    });
    $("body").click(function (e) {
        if (!(e.target.className == "account_compare_lnk")) {
            if ($(e.target).closest(".account_compare").length == 0) $(".account_compare").fadeOut(100);
        }
    });


    $("#auth_link").click(function (event) {
        $("#auth").dialog("open");
        event.preventDefault();
    });
    $(".lnk_logout").click(function (event) {
        //$("#logout").dialog("open");
        $("#popup_rating").dialog("open");
        event.preventDefault();
    });



    $("#auth").dialog({
        dialogClass: "ui-dialog_ie",
        autoOpen: false,
        closeText: "Закрыть",
        draggable: false,
        width: 424
    });
    $("#logout").dialog({
        dialogClass: "small ui-dialog_ie",
        autoOpen: false,
        closeText: "Закрыть",
        draggable: false,
        modal: true,
        width: 310
    });

    $("#popup_rating").dialog({
        dialogClass: "ui-dialog_ie popup_rating",
        autoOpen: false,
        closeText: "Закрыть",
        draggable: false,
        modal: true,
        width: 700
    });

    $("input.star").rating();

    $("#popup_rating .form_field1").blur(function () {
        if ($(this).text() == '') {
            $(this).text('Комментарии и пожелания по улучшению удобства и простоты пользования');
        }
    });
    $("#popup_rating .form_field1").focus(function () {
        if ($(this).text() == 'Комментарии и пожелания по улучшению удобства и простоты пользования') {
            $(this).text('');
        }
    });

    $(".tl_collapse > div").click(function () {
        $(".tl_bg_wrapper").slideToggle(function () {
            $(".tl_collapse").toggleClass("expanded");
            if ($(".tl_collapse").hasClass("expanded")) {
                $(".tl_collapse > div a").text("Свернуть");
            } else {
                $(".tl_collapse > div a").text("Развернуть таймлайн");
            }
        });
    });

    $(".brief_dialog_settings .filter input").val("Поиск акций");
    $(".brief_dialog_settings .filter input").blur(function () {
        //$(this).parent("div").removeClass("focused");
        if ($(this).val() == '') $(this).val('Поиск акций');
    });
    $(".brief_dialog_settings .filter input").focus(function () {
        //$(this).parent("div").addClass("focused");
        if ($(this).val() == 'Поиск акций') $(this).val('');
    });

    // Datepicker hiding by pressing ESC button and clicking elsewhere
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            $(".datepicker_place:visible").fadeOut(100);
        }
    });

    $("body").click(function (e) {
        if (!$(e.target.parentElement).hasClass("datepicker_wrap")) {
            $(".datepicker_place:visible").fadeOut(100);
        }
    });
    $(".datepicker_wrap").click(function (event) {
        event.stopPropagation();
    });

    if ($(".b_percent").length) {
        $(".b_percent").each(function () {
            var val = $(this).attr("rel") * 1;
            var newval = "sector";
            if (val > 0 && val < 7) {
                newval = newval + "16";
            }
            else if (val > 6 && val < 14) {
                newval = newval + "15";
            }
            else if (val > 13 && val < 21) {
                newval = newval + "14";
            }
            else if (val > 20 && val < 28) {
                newval = newval + "13";
            }
            else if (val > 27 && val < 35) {
                newval = newval + "12";
            }
            else if (val > 34 && val < 42) {
                newval = newval + "11";
            }
            else if (val > 41 && val < 49) {
                newval = newval + "10";
            }
            else if (val > 48 && val < 56) {
                newval = newval + "9";
            }
            else if (val > 55 && val < 63) {
                newval = newval + "8";
            }
            else if (val > 62 && val < 70) {
                newval = newval + "7";
            }
            else if (val > 69 && val < 77) {
                newval = newval + "6";
            }
            else if (val > 76 && val < 84) {
                newval = newval + "5";
            }
            else if (val > 83 && val < 91) {
                newval = newval + "4";
            }
            else if (val > 90 && val < 98) {
                newval = newval + "3";
            }
            else if (val > 96 && val < 100) {
                newval = newval + "2";
            }
            else if (val == 100) {
                newval = newval + "1";
            }
            $(this).addClass(newval);
        });
    }

    $(".btn_expand_wrap").click(function () {
        $(this).toggleClass("collapsed");
        if ($(this).hasClass("collapsed")) {
            $(this).find("a").text("Показать все");
        } else {
            $(this).find("a").text("Свернуть");
        }
    });

    $('#inputDate').DatePicker({
        format: 'd.m.Y',
        date: $('#inputDate').val(),
        current: $('#inputDate').val(),
        starts: 1,
        position: 'r',
        locale: {
            days: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"],
            daysShort: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
            daysMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
            months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
            monthsShort: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"],
        },
        onBeforeShow: function () {
            $('#inputDate').DatePickerSetDate($('#inputDate').val(), true);
        },
        onChange: function (formated, dates) {
            $('#inputDate').val(formated);
            $('#inputDate').DatePickerHide();
        }
    });
    cuSel({
        changedEl: ".cusel3",
        visRows: 5
    });
    cuSel({
        changedEl: ".cusel4",
        visRows: 5
    });

    if ($(".settings_wrap").width()) {
        for (var i = 1; i < $(".b_settings").size() ; i = i + 2) {
            var prevIndex = i - 1;
            var currBlock = $(".b_settings:eq(" + i + ")");
            var prevBlock = $(".b_settings:eq(" + prevIndex + ")");
            if (currBlock.height() < prevBlock.height()) {
                currBlock.height(prevBlock.height());
            } else {
                prevBlock.height(currBlock.height());
            }
        }
    }
});

$(window).scroll(function () {
    if ($(window).scrollTop() > 78) {
        $(".mm_wrap").addClass("mm_float");
    } else {
        $(".mm_wrap").removeClass("mm_float");
    }
});
