/* Theme Name: Worthy - Free Powerful Theme by HtmlCoder
 * Author:HtmlCoder
 * Author URI:http://www.htmlcoder.me
 * Version:1.0.0
 * Created:November 2014
 * License: Creative Commons Attribution 3.0 License (https://creativecommons.org/licenses/by/3.0/)
 * File Description: Initializations of plugins 
 */

/*
依赖：
bootstrap.min.css
jquery.backstretch.min.js
header.css
*/

(function ($) {
    $(document).ready(function () {

        // Fixed header
        //-----------------------------------------------
        $(window).scroll(function () {
            if (($(".header.fixed").length > 0)) {
                if (($(this).scrollTop() > 0) && ($(window).width() > 767)) {
                    $("body").addClass("fixed-header-on");
                } else {
                    $("body").removeClass("fixed-header-on");
                }
            };
        });

        $(window).load(function () {
            if (($(".header.fixed").length > 0)) {
                if (($(this).scrollTop() > 0) && ($(window).width() > 767)) {
                    $("body").addClass("fixed-header-on");
                } else {
                    $("body").removeClass("fixed-header-on");
                }
            };
        });

        //Scroll Spy
        //-----------------------------------------------
        if ($(".scrollspy").length > 0) {
            $("body").addClass("scroll-spy");
            $('body').scrollspy({
                target: '.scrollspy',
                offset: 152
            });
        }

        var $liNodes = $("#a-nav ul li");
        for (var i = 0; i < $liNodes.length; i++) {
            var $liNode = $($liNodes[i]);
            var refid = $liNode.find("a").attr("ref-id");
            if (refid) {
                var $mapNode = $("#" + refid);
                if ($mapNode.length > 0) {
                    $liNodes.removeClass("active");
                    $liNode.addClass("active");
                    break;
                }
            }
        }
    });
})(this.jQuery);