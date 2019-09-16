/*
依赖：
bootstrap.min.css
font-awesome.min.css 
*/

$(document).ready(function () {

    appendIcon();

    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('#back-top').fadeIn(200);
        } else {
            $('#back-top').fadeOut(200);
        }
    });

    $('#back-top').click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 300);
    })

    function appendIcon() {
        var backTopStyle = {};
        backTopStyle['background-color'] = '#eee';
        backTopStyle['bottom'] = '2em';
        backTopStyle['right'] = '2em';
        backTopStyle['color'] = '#111112';
        backTopStyle['font-size'] = '22px';
        backTopStyle['display'] = 'none';
        backTopStyle['position'] = 'fixed';
        backTopStyle['text-decoration'] = 'none';
        backTopStyle['width'] = '50px';
        backTopStyle['height'] = '50px';
        backTopStyle['line-height'] = '50px';
        backTopStyle['text-align'] = 'center';
        backTopStyle['transition'] = 'all 0.4s ease-in-out';

        var backTopStyle_hover = {};
        backTopStyle_hover['background'] = '#f1c11a';
        backTopStyle_hover['color'] = '#ffffff';

        var backTopStyleHtml = '';
        backTopStyleHtml += '<style>';
        backTopStyleHtml += toStyleFormatHtml('#back-top', backTopStyle);
        backTopStyleHtml += toStyleFormatHtml('#back-top:hover', backTopStyle_hover);
        backTopStyleHtml += '</style>';

        var body = $("html body");
        body.append(backTopStyleHtml);
        body.append('<a href="#" id="back-top"><i class="fa fa-angle-up"></i></a>');
    }
    function toStyleFormatHtml(className, styleJson) {
        var html = '';
        html += className;

        html += '{';
        for (var prop in styleJson) {
            html += prop + ': ' + styleJson[prop] + ';';
        }
        html += '}';

        return html;
    }
});
