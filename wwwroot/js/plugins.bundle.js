/*!
 * The Final Countdown for jQuery v2.1.0 (http://hilios.github.io/jQuery.countdown/)
 * Copyright (c) 2015 Edson Hilios
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
!function (a) {
    "use strict";
    "function" == typeof define && define.amd ? define(["jquery"], a) : a(jQuery)
}(function (a) {
    "use strict";

    function b(a) {
        if (a instanceof Date) return a;
        if (String(a).match(g)) return String(a).match(/^[0-9]*$/) && (a = Number(a)), String(a).match(/\-/) && (a = String(a).replace(/\-/g, "/")), new Date(a);
        throw new Error("Couldn't cast `" + a + "` to a date object.")
    }

    function c(a) {
        var b = a.toString().replace(/([.?*+^$[\]\\(){}|-])/g, "\\$1");
        return new RegExp(b)
    }

    function d(a) {
        return function (b) {
            var d = b.match(/%(-|!)?[A-Z]{1}(:[^;]+;)?/gi);
            if (d) for (var f = 0, g = d.length; g > f; ++f) {
                var h = d[f].match(/%(-|!)?([a-zA-Z]{1})(:[^;]+;)?/), j = c(h[0]), k = h[1] || "", l = h[3] || "",
                    m = null;
                h = h[2], i.hasOwnProperty(h) && (m = i[h], m = Number(a[m])), null !== m && ("!" === k && (m = e(l, m)), "" === k && 10 > m && (m = "0" + m.toString()), b = b.replace(j, m.toString()))
            }
            return b = b.replace(/%%/, "%")
        }
    }

    function e(a, b) {
        var c = "s", d = "";
        return a && (a = a.replace(/(:|;|\s)/gi, "").split(/\,/), 1 === a.length ? c = a[0] : (d = a[0], c = a[1])), 1 === Math.abs(b) ? d : c
    }

    var f = [], g = [], h = {precision: 100, elapse: !1};
    g.push(/^[0-9]*$/.source), g.push(/([0-9]{1,2}\/){2}[0-9]{4}( [0-9]{1,2}(:[0-9]{2}){2})?/.source), g.push(/[0-9]{4}([\/\-][0-9]{1,2}){2}( [0-9]{1,2}(:[0-9]{2}){2})?/.source), g = new RegExp(g.join("|"));
    var i = {
        Y: "years",
        m: "months",
        n: "daysToMonth",
        w: "weeks",
        d: "daysToWeek",
        D: "totalDays",
        H: "hours",
        M: "minutes",
        S: "seconds"
    }, j = function (b, c, d) {
        this.el = b, this.$el = a(b), this.interval = null, this.offset = {}, this.options = a.extend({}, h), this.instanceNumber = f.length, f.push(this), this.$el.data("countdown-instance", this.instanceNumber), d && ("function" == typeof d ? (this.$el.on("update.countdown", d), this.$el.on("stoped.countdown", d), this.$el.on("finish.countdown", d)) : this.options = a.extend({}, h, d)), this.setFinalDate(c), this.start()
    };
    a.extend(j.prototype, {
        start: function () {
            null !== this.interval && clearInterval(this.interval);
            var a = this;
            this.update(), this.interval = setInterval(function () {
                a.update.call(a)
            }, this.options.precision)
        }, stop: function () {
            clearInterval(this.interval), this.interval = null, this.dispatchEvent("stoped")
        }, toggle: function () {
            this.interval ? this.stop() : this.start()
        }, pause: function () {
            this.stop()
        }, resume: function () {
            this.start()
        }, remove: function () {
            this.stop.call(this), f[this.instanceNumber] = null, delete this.$el.data().countdownInstance
        }, setFinalDate: function (a) {
            this.finalDate = b(a)
        }, update: function () {
            if (0 === this.$el.closest("html").length) return void this.remove();
            var b, c = void 0 !== a._data(this.el, "events"), d = new Date;
            b = this.finalDate.getTime() - d.getTime(), b = Math.ceil(b / 1e3), b = !this.options.elapse && 0 > b ? 0 : Math.abs(b), this.totalSecsLeft !== b && c && (this.totalSecsLeft = b, this.elapsed = d >= this.finalDate, this.offset = {
                seconds: this.totalSecsLeft % 60,
                minutes: Math.floor(this.totalSecsLeft / 60) % 60,
                hours: Math.floor(this.totalSecsLeft / 60 / 60) % 24,
                days: Math.floor(this.totalSecsLeft / 60 / 60 / 24) % 7,
                daysToWeek: Math.floor(this.totalSecsLeft / 60 / 60 / 24) % 7,
                daysToMonth: Math.floor(this.totalSecsLeft / 60 / 60 / 24 % 30.4368),
                totalDays: Math.floor(this.totalSecsLeft / 60 / 60 / 24),
                weeks: Math.floor(this.totalSecsLeft / 60 / 60 / 24 / 7),
                months: Math.floor(this.totalSecsLeft / 60 / 60 / 24 / 30.4368),
                years: Math.abs(this.finalDate.getFullYear() - d.getFullYear())
            }, this.options.elapse || 0 !== this.totalSecsLeft ? this.dispatchEvent("update") : (this.stop(), this.dispatchEvent("finish")))
        }, dispatchEvent: function (b) {
            var c = a.Event(b + ".countdown");
            c.finalDate = this.finalDate, c.elapsed = this.elapsed, c.offset = a.extend({}, this.offset), c.strftime = d(this.offset), this.$el.trigger(c)
        }
    }), a.fn.countdown = function () {
        var b = Array.prototype.slice.call(arguments, 0);
        return this.each(function () {
            var c = a(this).data("countdown-instance");
            if (void 0 !== c) {
                var d = f[c], e = b[0];
                j.prototype.hasOwnProperty(e) ? d[e].apply(d, b.slice(1)) : null === String(e).match(/^[$A-Z_][0-9A-Z_$]*$/i) ? (d.setFinalDate.call(d, e), d.start()) : a.error("Method %s does not exist on jQuery.countdown".replace(/\%s/gi, e))
            } else new j(this, b[0], b[1])
        })
    }
});
!function (e) {
    e.fn.appear = function (a, r) {
        var p = e.extend({data: void 0, one: !0, accX: 0, accY: 0}, r);
        return this.each(function () {
            var r = e(this);
            if (r.appeared = !1, a) {
                var n = e(window), t = function () {
                    if (r.is(":visible")) {
                        var e = n.scrollLeft(), a = n.scrollTop(), t = r.offset(), c = t.left, i = t.top, o = p.accX,
                            f = p.accY, s = r.height(), l = n.height(), h = r.width(), d = n.width();
                        i + s + f >= a && i <= a + l + f && c + h + o >= e && c <= e + d + o ? r.appeared || r.trigger("appear", p.data) : r.appeared = !1
                    } else r.appeared = !1
                }, c = function () {
                    if (r.appeared = !0, p.one) {
                        n.unbind("scroll", t);
                        var c = e.inArray(t, e.fn.appear.checks);
                        c >= 0 && e.fn.appear.checks.splice(c, 1)
                    }
                    a.apply(this, arguments)
                };
                p.one ? r.one("appear", p.data, c) : r.bind("appear", p.data, c), n.scroll(t), e.fn.appear.checks.push(t), t()
            } else r.trigger("appear", p.data)
        })
    }, e.extend(e.fn.appear, {
        checks: [], timeout: null, checkAll: function () {
            var a = e.fn.appear.checks.length;
            if (a > 0) for (; a--;) e.fn.appear.checks[a]()
        }, run: function () {
            e.fn.appear.timeout && clearTimeout(e.fn.appear.timeout), e.fn.appear.timeout = setTimeout(e.fn.appear.checkAll, 20)
        }
    }), e.each(["append", "prepend", "after", "before", "attr", "removeAttr", "addClass", "removeClass", "toggleClass", "remove", "css", "show", "hide"], function (a, r) {
        var p = e.fn[r];
        p && (e.fn[r] = function () {
            var a = p.apply(this, arguments);
            return e.fn.appear.run(), a
        })
    })
}(jQuery);
"use strict";
jQuery,
    jQuery(document).ready(function (o) {
        0 < o(".offset-side-bar").length &&
        o(".offset-side-bar").on("click", function (e) {
            e.preventDefault(), e.stopPropagation(), o(".cart-group").addClass("isActive");
        }),
        0 < o(".close-side-widget").length &&
        o(".close-side-widget").on("click", function (e) {
            e.preventDefault(), o(".cart-group").removeClass("isActive");
        }),
        0 < o(".navSidebar-button").length &&
        o(".navSidebar-button").on("click", function (e) {
            e.preventDefault(), e.stopPropagation(), o(".info-group").addClass("isActive");
        }),
        0 < o(".close-side-widget").length &&
        o(".close-side-widget").on("click", function (e) {
            e.preventDefault(), o(".info-group").removeClass("isActive");
        }),
            o("body").on("click", function (e) {
                o(".info-group").removeClass("isActive"), o(".cart-group").removeClass("isActive");
            }),
            o(".xs-sidebar-widget").on("click", function (e) {
                e.stopPropagation();
            }),
        0 < o(".xs-modal-popup").length &&
        o(".xs-modal-popup").magnificPopup({
            type: "inline",
            fixedContentPos: !1,
            fixedBgPos: !0,
            overflowY: "auto",
            closeBtnInside: !1,
            callbacks: {
                beforeOpen: function () {
                    this.st.mainClass = "my-mfp-slide-bottom xs-promo-popup";
                },
            },
        });
    });
/**
 * Owl Carousel v2.3.4
 * Copyright 2013-2018 David Deutsch
 * Licensed under: SEE LICENSE IN https://github.com/OwlCarousel2/OwlCarousel2/blob/master/LICENSE
 */
!function (a, b, c, d) {
    function e(b, c) {
        this.settings = null, this.options = a.extend({}, e.Defaults, c), this.$element = a(b), this._handlers = {}, this._plugins = {}, this._supress = {}, this._current = null, this._speed = null, this._coordinates = [], this._breakpoint = null, this._width = null, this._items = [], this._clones = [], this._mergers = [], this._widths = [], this._invalidated = {}, this._pipe = [], this._drag = {
            time: null,
            target: null,
            pointer: null,
            stage: {start: null, current: null},
            direction: null
        }, this._states = {
            current: {},
            tags: {initializing: ["busy"], animating: ["busy"], dragging: ["interacting"]}
        }, a.each(["onResize", "onThrottledResize"], a.proxy(function (b, c) {
            this._handlers[c] = a.proxy(this[c], this)
        }, this)), a.each(e.Plugins, a.proxy(function (a, b) {
            this._plugins[a.charAt(0).toLowerCase() + a.slice(1)] = new b(this)
        }, this)), a.each(e.Workers, a.proxy(function (b, c) {
            this._pipe.push({filter: c.filter, run: a.proxy(c.run, this)})
        }, this)), this.setup(), this.initialize()
    }

    e.Defaults = {
        items: 3,
        loop: !1,
        center: !1,
        rewind: !1,
        checkVisibility: !0,
        mouseDrag: !0,
        touchDrag: !0,
        pullDrag: !0,
        freeDrag: !1,
        margin: 0,
        stagePadding: 0,
        merge: !1,
        mergeFit: !0,
        autoWidth: !1,
        startPosition: 0,
        rtl: !1,
        smartSpeed: 250,
        fluidSpeed: !1,
        dragEndSpeed: !1,
        responsive: {},
        responsiveRefreshRate: 200,
        responsiveBaseElement: b,
        fallbackEasing: "swing",
        slideTransition: "",
        info: !1,
        nestedItemSelector: !1,
        itemElement: "div",
        stageElement: "div",
        refreshClass: "owl-refresh",
        loadedClass: "owl-loaded",
        loadingClass: "owl-loading",
        rtlClass: "owl-rtl",
        responsiveClass: "owl-responsive",
        dragClass: "owl-drag",
        itemClass: "owl-item",
        stageClass: "owl-stage",
        stageOuterClass: "owl-stage-outer",
        grabClass: "owl-grab"
    }, e.Width = {Default: "default", Inner: "inner", Outer: "outer"}, e.Type = {
        Event: "event",
        State: "state"
    }, e.Plugins = {}, e.Workers = [{
        filter: ["width", "settings"], run: function () {
            this._width = this.$element.width()
        }
    }, {
        filter: ["width", "items", "settings"], run: function (a) {
            a.current = this._items && this._items[this.relative(this._current)]
        }
    }, {
        filter: ["items", "settings"], run: function () {
            this.$stage.children(".cloned").remove()
        }
    }, {
        filter: ["width", "items", "settings"], run: function (a) {
            var b = this.settings.margin || "", c = !this.settings.autoWidth, d = this.settings.rtl,
                e = {width: "auto", "margin-left": d ? b : "", "margin-right": d ? "" : b};
            !c && this.$stage.children().css(e), a.css = e
        }
    }, {
        filter: ["width", "items", "settings"], run: function (a) {
            var b = (this.width() / this.settings.items).toFixed(3) - this.settings.margin, c = null,
                d = this._items.length, e = !this.settings.autoWidth, f = [];
            for (a.items = {
                merge: !1,
                width: b
            }; d--;) c = this._mergers[d], c = this.settings.mergeFit && Math.min(c, this.settings.items) || c, a.items.merge = c > 1 || a.items.merge, f[d] = e ? b * c : this._items[d].width();
            this._widths = f
        }
    }, {
        filter: ["items", "settings"], run: function () {
            var b = [], c = this._items, d = this.settings, e = Math.max(2 * d.items, 4),
                f = 2 * Math.ceil(c.length / 2), g = d.loop && c.length ? d.rewind ? e : Math.max(e, f) : 0, h = "",
                i = "";
            for (g /= 2; g > 0;) b.push(this.normalize(b.length / 2, !0)), h += c[b[b.length - 1]][0].outerHTML, b.push(this.normalize(c.length - 1 - (b.length - 1) / 2, !0)), i = c[b[b.length - 1]][0].outerHTML + i, g -= 1;
            this._clones = b, a(h).addClass("cloned").appendTo(this.$stage), a(i).addClass("cloned").prependTo(this.$stage)
        }
    }, {
        filter: ["width", "items", "settings"], run: function () {
            for (var a = this.settings.rtl ? 1 : -1, b = this._clones.length + this._items.length, c = -1, d = 0, e = 0, f = []; ++c < b;) d = f[c - 1] || 0, e = this._widths[this.relative(c)] + this.settings.margin, f.push(d + e * a);
            this._coordinates = f
        }
    }, {
        filter: ["width", "items", "settings"], run: function () {
            var a = this.settings.stagePadding, b = this._coordinates, c = {
                width: Math.ceil(Math.abs(b[b.length - 1])) + 2 * a,
                "padding-left": a || "",
                "padding-right": a || ""
            };
            this.$stage.css(c)
        }
    }, {
        filter: ["width", "items", "settings"], run: function (a) {
            var b = this._coordinates.length, c = !this.settings.autoWidth, d = this.$stage.children();
            if (c && a.items.merge) for (; b--;) a.css.width = this._widths[this.relative(b)], d.eq(b).css(a.css); else c && (a.css.width = a.items.width, d.css(a.css))
        }
    }, {
        filter: ["items"], run: function () {
            this._coordinates.length < 1 && this.$stage.removeAttr("style")
        }
    }, {
        filter: ["width", "items", "settings"], run: function (a) {
            a.current = a.current ? this.$stage.children().index(a.current) : 0, a.current = Math.max(this.minimum(), Math.min(this.maximum(), a.current)), this.reset(a.current)
        }
    }, {
        filter: ["position"], run: function () {
            this.animate(this.coordinates(this._current))
        }
    }, {
        filter: ["width", "position", "items", "settings"], run: function () {
            var a, b, c, d, e = this.settings.rtl ? 1 : -1, f = 2 * this.settings.stagePadding,
                g = this.coordinates(this.current()) + f, h = g + this.width() * e, i = [];
            for (c = 0, d = this._coordinates.length; c < d; c++) a = this._coordinates[c - 1] || 0, b = Math.abs(this._coordinates[c]) + f * e, (this.op(a, "<=", g) && this.op(a, ">", h) || this.op(b, "<", g) && this.op(b, ">", h)) && i.push(c);
            this.$stage.children(".active").removeClass("active"), this.$stage.children(":eq(" + i.join("), :eq(") + ")").addClass("active"), this.$stage.children(".center").removeClass("center"), this.settings.center && this.$stage.children().eq(this.current()).addClass("center")
        }
    }], e.prototype.initializeStage = function () {
        this.$stage = this.$element.find("." + this.settings.stageClass), this.$stage.length || (this.$element.addClass(this.options.loadingClass), this.$stage = a("<" + this.settings.stageElement + ">", {class: this.settings.stageClass}).wrap(a("<div/>", {class: this.settings.stageOuterClass})), this.$element.append(this.$stage.parent()))
    }, e.prototype.initializeItems = function () {
        var b = this.$element.find(".owl-item");
        if (b.length) return this._items = b.get().map(function (b) {
            return a(b)
        }), this._mergers = this._items.map(function () {
            return 1
        }), void this.refresh();
        this.replace(this.$element.children().not(this.$stage.parent())), this.isVisible() ? this.refresh() : this.invalidate("width"), this.$element.removeClass(this.options.loadingClass).addClass(this.options.loadedClass)
    }, e.prototype.initialize = function () {
        if (this.enter("initializing"), this.trigger("initialize"), this.$element.toggleClass(this.settings.rtlClass, this.settings.rtl), this.settings.autoWidth && !this.is("pre-loading")) {
            var a, b, c;
            a = this.$element.find("img"), b = this.settings.nestedItemSelector ? "." + this.settings.nestedItemSelector : d, c = this.$element.children(b).width(), a.length && c <= 0 && this.preloadAutoWidthImages(a)
        }
        this.initializeStage(), this.initializeItems(), this.registerEventHandlers(), this.leave("initializing"), this.trigger("initialized")
    }, e.prototype.isVisible = function () {
        return !this.settings.checkVisibility || this.$element.is(":visible")
    }, e.prototype.setup = function () {
        var b = this.viewport(), c = this.options.responsive, d = -1, e = null;
        c ? (a.each(c, function (a) {
            a <= b && a > d && (d = Number(a))
        }), e = a.extend({}, this.options, c[d]), "function" == typeof e.stagePadding && (e.stagePadding = e.stagePadding()), delete e.responsive, e.responsiveClass && this.$element.attr("class", this.$element.attr("class").replace(new RegExp("(" + this.options.responsiveClass + "-)\\S+\\s", "g"), "$1" + d))) : e = a.extend({}, this.options), this.trigger("change", {
            property: {
                name: "settings",
                value: e
            }
        }), this._breakpoint = d, this.settings = e, this.invalidate("settings"), this.trigger("changed", {
            property: {
                name: "settings",
                value: this.settings
            }
        })
    }, e.prototype.optionsLogic = function () {
        this.settings.autoWidth && (this.settings.stagePadding = !1, this.settings.merge = !1)
    }, e.prototype.prepare = function (b) {
        var c = this.trigger("prepare", {content: b});
        return c.data || (c.data = a("<" + this.settings.itemElement + "/>").addClass(this.options.itemClass).append(b)), this.trigger("prepared", {content: c.data}), c.data
    }, e.prototype.update = function () {
        for (var b = 0, c = this._pipe.length, d = a.proxy(function (a) {
            return this[a]
        }, this._invalidated), e = {}; b < c;) (this._invalidated.all || a.grep(this._pipe[b].filter, d).length > 0) && this._pipe[b].run(e), b++;
        this._invalidated = {}, !this.is("valid") && this.enter("valid")
    }, e.prototype.width = function (a) {
        switch (a = a || e.Width.Default) {
            case e.Width.Inner:
            case e.Width.Outer:
                return this._width;
            default:
                return this._width - 2 * this.settings.stagePadding + this.settings.margin
        }
    }, e.prototype.refresh = function () {
        this.enter("refreshing"), this.trigger("refresh"), this.setup(), this.optionsLogic(), this.$element.addClass(this.options.refreshClass), this.update(), this.$element.removeClass(this.options.refreshClass), this.leave("refreshing"), this.trigger("refreshed")
    }, e.prototype.onThrottledResize = function () {
        b.clearTimeout(this.resizeTimer), this.resizeTimer = b.setTimeout(this._handlers.onResize, this.settings.responsiveRefreshRate)
    }, e.prototype.onResize = function () {
        return !!this._items.length && (this._width !== this.$element.width() && (!!this.isVisible() && (this.enter("resizing"), this.trigger("resize").isDefaultPrevented() ? (this.leave("resizing"), !1) : (this.invalidate("width"), this.refresh(), this.leave("resizing"), void this.trigger("resized")))))
    }, e.prototype.registerEventHandlers = function () {
        a.support.transition && this.$stage.on(a.support.transition.end + ".owl.core", a.proxy(this.onTransitionEnd, this)), !1 !== this.settings.responsive && this.on(b, "resize", this._handlers.onThrottledResize), this.settings.mouseDrag && (this.$element.addClass(this.options.dragClass), this.$stage.on("mousedown.owl.core", a.proxy(this.onDragStart, this)), this.$stage.on("dragstart.owl.core selectstart.owl.core", function () {
            return !1
        })), this.settings.touchDrag && (this.$stage.on("touchstart.owl.core", a.proxy(this.onDragStart, this)), this.$stage.on("touchcancel.owl.core", a.proxy(this.onDragEnd, this)))
    }, e.prototype.onDragStart = function (b) {
        var d = null;
        3 !== b.which && (a.support.transform ? (d = this.$stage.css("transform").replace(/.*\(|\)| /g, "").split(","), d = {
            x: d[16 === d.length ? 12 : 4],
            y: d[16 === d.length ? 13 : 5]
        }) : (d = this.$stage.position(), d = {
            x: this.settings.rtl ? d.left + this.$stage.width() - this.width() + this.settings.margin : d.left,
            y: d.top
        }), this.is("animating") && (a.support.transform ? this.animate(d.x) : this.$stage.stop(), this.invalidate("position")), this.$element.toggleClass(this.options.grabClass, "mousedown" === b.type), this.speed(0), this._drag.time = (new Date).getTime(), this._drag.target = a(b.target), this._drag.stage.start = d, this._drag.stage.current = d, this._drag.pointer = this.pointer(b), a(c).on("mouseup.owl.core touchend.owl.core", a.proxy(this.onDragEnd, this)), a(c).one("mousemove.owl.core touchmove.owl.core", a.proxy(function (b) {
            var d = this.difference(this._drag.pointer, this.pointer(b));
            a(c).on("mousemove.owl.core touchmove.owl.core", a.proxy(this.onDragMove, this)), Math.abs(d.x) < Math.abs(d.y) && this.is("valid") || (b.preventDefault(), this.enter("dragging"), this.trigger("drag"))
        }, this)))
    }, e.prototype.onDragMove = function (a) {
        var b = null, c = null, d = null, e = this.difference(this._drag.pointer, this.pointer(a)),
            f = this.difference(this._drag.stage.start, e);
        this.is("dragging") && (a.preventDefault(), this.settings.loop ? (b = this.coordinates(this.minimum()), c = this.coordinates(this.maximum() + 1) - b, f.x = ((f.x - b) % c + c) % c + b) : (b = this.settings.rtl ? this.coordinates(this.maximum()) : this.coordinates(this.minimum()), c = this.settings.rtl ? this.coordinates(this.minimum()) : this.coordinates(this.maximum()), d = this.settings.pullDrag ? -1 * e.x / 5 : 0, f.x = Math.max(Math.min(f.x, b + d), c + d)), this._drag.stage.current = f, this.animate(f.x))
    }, e.prototype.onDragEnd = function (b) {
        var d = this.difference(this._drag.pointer, this.pointer(b)), e = this._drag.stage.current,
            f = d.x > 0 ^ this.settings.rtl ? "left" : "right";
        a(c).off(".owl.core"), this.$element.removeClass(this.options.grabClass), (0 !== d.x && this.is("dragging") || !this.is("valid")) && (this.speed(this.settings.dragEndSpeed || this.settings.smartSpeed), this.current(this.closest(e.x, 0 !== d.x ? f : this._drag.direction)), this.invalidate("position"), this.update(), this._drag.direction = f, (Math.abs(d.x) > 3 || (new Date).getTime() - this._drag.time > 300) && this._drag.target.one("click.owl.core", function () {
            return !1
        })), this.is("dragging") && (this.leave("dragging"), this.trigger("dragged"))
    }, e.prototype.closest = function (b, c) {
        var e = -1, f = 30, g = this.width(), h = this.coordinates();
        return this.settings.freeDrag || a.each(h, a.proxy(function (a, i) {
            return "left" === c && b > i - f && b < i + f ? e = a : "right" === c && b > i - g - f && b < i - g + f ? e = a + 1 : this.op(b, "<", i) && this.op(b, ">", h[a + 1] !== d ? h[a + 1] : i - g) && (e = "left" === c ? a + 1 : a), -1 === e
        }, this)), this.settings.loop || (this.op(b, ">", h[this.minimum()]) ? e = b = this.minimum() : this.op(b, "<", h[this.maximum()]) && (e = b = this.maximum())), e
    }, e.prototype.animate = function (b) {
        var c = this.speed() > 0;
        this.is("animating") && this.onTransitionEnd(), c && (this.enter("animating"), this.trigger("translate")), a.support.transform3d && a.support.transition ? this.$stage.css({
            transform: "translate3d(" + b + "px,0px,0px)",
            transition: this.speed() / 1e3 + "s" + (this.settings.slideTransition ? " " + this.settings.slideTransition : "")
        }) : c ? this.$stage.animate({left: b + "px"}, this.speed(), this.settings.fallbackEasing, a.proxy(this.onTransitionEnd, this)) : this.$stage.css({left: b + "px"})
    }, e.prototype.is = function (a) {
        return this._states.current[a] && this._states.current[a] > 0
    }, e.prototype.current = function (a) {
        if (a === d) return this._current;
        if (0 === this._items.length) return d;
        if (a = this.normalize(a), this._current !== a) {
            var b = this.trigger("change", {property: {name: "position", value: a}});
            b.data !== d && (a = this.normalize(b.data)), this._current = a, this.invalidate("position"), this.trigger("changed", {
                property: {
                    name: "position",
                    value: this._current
                }
            })
        }
        return this._current
    }, e.prototype.invalidate = function (b) {
        return "string" === a.type(b) && (this._invalidated[b] = !0, this.is("valid") && this.leave("valid")), a.map(this._invalidated, function (a, b) {
            return b
        })
    }, e.prototype.reset = function (a) {
        (a = this.normalize(a)) !== d && (this._speed = 0, this._current = a, this.suppress(["translate", "translated"]), this.animate(this.coordinates(a)), this.release(["translate", "translated"]))
    }, e.prototype.normalize = function (a, b) {
        var c = this._items.length, e = b ? 0 : this._clones.length;
        return !this.isNumeric(a) || c < 1 ? a = d : (a < 0 || a >= c + e) && (a = ((a - e / 2) % c + c) % c + e / 2), a
    }, e.prototype.relative = function (a) {
        return a -= this._clones.length / 2, this.normalize(a, !0)
    }, e.prototype.maximum = function (a) {
        var b, c, d, e = this.settings, f = this._coordinates.length;
        if (e.loop) f = this._clones.length / 2 + this._items.length - 1; else if (e.autoWidth || e.merge) {
            if (b = this._items.length) for (c = this._items[--b].width(), d = this.$element.width(); b-- && !((c += this._items[b].width() + this.settings.margin) > d);) ;
            f = b + 1
        } else f = e.center ? this._items.length - 1 : this._items.length - e.items;
        return a && (f -= this._clones.length / 2), Math.max(f, 0)
    }, e.prototype.minimum = function (a) {
        return a ? 0 : this._clones.length / 2
    }, e.prototype.items = function (a) {
        return a === d ? this._items.slice() : (a = this.normalize(a, !0), this._items[a])
    }, e.prototype.mergers = function (a) {
        return a === d ? this._mergers.slice() : (a = this.normalize(a, !0), this._mergers[a])
    }, e.prototype.clones = function (b) {
        var c = this._clones.length / 2, e = c + this._items.length, f = function (a) {
            return a % 2 == 0 ? e + a / 2 : c - (a + 1) / 2
        };
        return b === d ? a.map(this._clones, function (a, b) {
            return f(b)
        }) : a.map(this._clones, function (a, c) {
            return a === b ? f(c) : null
        })
    }, e.prototype.speed = function (a) {
        return a !== d && (this._speed = a), this._speed
    }, e.prototype.coordinates = function (b) {
        var c, e = 1, f = b - 1;
        return b === d ? a.map(this._coordinates, a.proxy(function (a, b) {
            return this.coordinates(b)
        }, this)) : (this.settings.center ? (this.settings.rtl && (e = -1, f = b + 1), c = this._coordinates[b], c += (this.width() - c + (this._coordinates[f] || 0)) / 2 * e) : c = this._coordinates[f] || 0, c = Math.ceil(c))
    }, e.prototype.duration = function (a, b, c) {
        return 0 === c ? 0 : Math.min(Math.max(Math.abs(b - a), 1), 6) * Math.abs(c || this.settings.smartSpeed)
    }, e.prototype.to = function (a, b) {
        var c = this.current(), d = null, e = a - this.relative(c), f = (e > 0) - (e < 0), g = this._items.length,
            h = this.minimum(), i = this.maximum();
        this.settings.loop ? (!this.settings.rewind && Math.abs(e) > g / 2 && (e += -1 * f * g), a = c + e, (d = ((a - h) % g + g) % g + h) !== a && d - e <= i && d - e > 0 && (c = d - e, a = d, this.reset(c))) : this.settings.rewind ? (i += 1, a = (a % i + i) % i) : a = Math.max(h, Math.min(i, a)), this.speed(this.duration(c, a, b)), this.current(a), this.isVisible() && this.update()
    }, e.prototype.next = function (a) {
        a = a || !1, this.to(this.relative(this.current()) + 1, a)
    }, e.prototype.prev = function (a) {
        a = a || !1, this.to(this.relative(this.current()) - 1, a)
    }, e.prototype.onTransitionEnd = function (a) {
        if (a !== d && (a.stopPropagation(), (a.target || a.srcElement || a.originalTarget) !== this.$stage.get(0))) return !1;
        this.leave("animating"), this.trigger("translated")
    }, e.prototype.viewport = function () {
        var d;
        return this.options.responsiveBaseElement !== b ? d = a(this.options.responsiveBaseElement).width() : b.innerWidth ? d = b.innerWidth : c.documentElement && c.documentElement.clientWidth ? d = c.documentElement.clientWidth : console.warn("Can not detect viewport width."), d
    }, e.prototype.replace = function (b) {
        this.$stage.empty(), this._items = [], b && (b = b instanceof jQuery ? b : a(b)), this.settings.nestedItemSelector && (b = b.find("." + this.settings.nestedItemSelector)), b.filter(function () {
            return 1 === this.nodeType
        }).each(a.proxy(function (a, b) {
            b = this.prepare(b), this.$stage.append(b), this._items.push(b), this._mergers.push(1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)
        }, this)), this.reset(this.isNumeric(this.settings.startPosition) ? this.settings.startPosition : 0), this.invalidate("items")
    }, e.prototype.add = function (b, c) {
        var e = this.relative(this._current);
        c = c === d ? this._items.length : this.normalize(c, !0), b = b instanceof jQuery ? b : a(b), this.trigger("add", {
            content: b,
            position: c
        }), b = this.prepare(b), 0 === this._items.length || c === this._items.length ? (0 === this._items.length && this.$stage.append(b), 0 !== this._items.length && this._items[c - 1].after(b), this._items.push(b), this._mergers.push(1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)) : (this._items[c].before(b), this._items.splice(c, 0, b), this._mergers.splice(c, 0, 1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)), this._items[e] && this.reset(this._items[e].index()), this.invalidate("items"), this.trigger("added", {
            content: b,
            position: c
        })
    }, e.prototype.remove = function (a) {
        (a = this.normalize(a, !0)) !== d && (this.trigger("remove", {
            content: this._items[a],
            position: a
        }), this._items[a].remove(), this._items.splice(a, 1), this._mergers.splice(a, 1), this.invalidate("items"), this.trigger("removed", {
            content: null,
            position: a
        }))
    }, e.prototype.preloadAutoWidthImages = function (b) {
        b.each(a.proxy(function (b, c) {
            this.enter("pre-loading"), c = a(c), a(new Image).one("load", a.proxy(function (a) {
                c.attr("src", a.target.src), c.css("opacity", 1), this.leave("pre-loading"), !this.is("pre-loading") && !this.is("initializing") && this.refresh()
            }, this)).attr("src", c.attr("src") || c.attr("data-src") || c.attr("data-src-retina"))
        }, this))
    }, e.prototype.destroy = function () {
        this.$element.off(".owl.core"), this.$stage.off(".owl.core"), a(c).off(".owl.core"), !1 !== this.settings.responsive && (b.clearTimeout(this.resizeTimer), this.off(b, "resize", this._handlers.onThrottledResize));
        for (var d in this._plugins) this._plugins[d].destroy();
        this.$stage.children(".cloned").remove(), this.$stage.unwrap(), this.$stage.children().contents().unwrap(), this.$stage.children().unwrap(), this.$stage.remove(), this.$element.removeClass(this.options.refreshClass).removeClass(this.options.loadingClass).removeClass(this.options.loadedClass).removeClass(this.options.rtlClass).removeClass(this.options.dragClass).removeClass(this.options.grabClass).attr("class", this.$element.attr("class").replace(new RegExp(this.options.responsiveClass + "-\\S+\\s", "g"), "")).removeData("owl.carousel")
    }, e.prototype.op = function (a, b, c) {
        var d = this.settings.rtl;
        switch (b) {
            case"<":
                return d ? a > c : a < c;
            case">":
                return d ? a < c : a > c;
            case">=":
                return d ? a <= c : a >= c;
            case"<=":
                return d ? a >= c : a <= c
        }
    }, e.prototype.on = function (a, b, c, d) {
        a.addEventListener ? a.addEventListener(b, c, d) : a.attachEvent && a.attachEvent("on" + b, c)
    }, e.prototype.off = function (a, b, c, d) {
        a.removeEventListener ? a.removeEventListener(b, c, d) : a.detachEvent && a.detachEvent("on" + b, c)
    }, e.prototype.trigger = function (b, c, d, f, g) {
        var h = {item: {count: this._items.length, index: this.current()}},
            i = a.camelCase(a.grep(["on", b, d], function (a) {
                return a
            }).join("-").toLowerCase()),
            j = a.Event([b, "owl", d || "carousel"].join(".").toLowerCase(), a.extend({relatedTarget: this}, h, c));
        return this._supress[b] || (a.each(this._plugins, function (a, b) {
            b.onTrigger && b.onTrigger(j)
        }), this.register({
            type: e.Type.Event,
            name: b
        }), this.$element.trigger(j), this.settings && "function" == typeof this.settings[i] && this.settings[i].call(this, j)), j
    }, e.prototype.enter = function (b) {
        a.each([b].concat(this._states.tags[b] || []), a.proxy(function (a, b) {
            this._states.current[b] === d && (this._states.current[b] = 0), this._states.current[b]++
        }, this))
    }, e.prototype.leave = function (b) {
        a.each([b].concat(this._states.tags[b] || []), a.proxy(function (a, b) {
            this._states.current[b]--
        }, this))
    }, e.prototype.register = function (b) {
        if (b.type === e.Type.Event) {
            if (a.event.special[b.name] || (a.event.special[b.name] = {}), !a.event.special[b.name].owl) {
                var c = a.event.special[b.name]._default;
                a.event.special[b.name]._default = function (a) {
                    return !c || !c.apply || a.namespace && -1 !== a.namespace.indexOf("owl") ? a.namespace && a.namespace.indexOf("owl") > -1 : c.apply(this, arguments)
                }, a.event.special[b.name].owl = !0
            }
        } else b.type === e.Type.State && (this._states.tags[b.name] ? this._states.tags[b.name] = this._states.tags[b.name].concat(b.tags) : this._states.tags[b.name] = b.tags, this._states.tags[b.name] = a.grep(this._states.tags[b.name], a.proxy(function (c, d) {
            return a.inArray(c, this._states.tags[b.name]) === d
        }, this)))
    }, e.prototype.suppress = function (b) {
        a.each(b, a.proxy(function (a, b) {
            this._supress[b] = !0
        }, this))
    }, e.prototype.release = function (b) {
        a.each(b, a.proxy(function (a, b) {
            delete this._supress[b]
        }, this))
    }, e.prototype.pointer = function (a) {
        var c = {x: null, y: null};
        return a = a.originalEvent || a || b.event, a = a.touches && a.touches.length ? a.touches[0] : a.changedTouches && a.changedTouches.length ? a.changedTouches[0] : a, a.pageX ? (c.x = a.pageX, c.y = a.pageY) : (c.x = a.clientX, c.y = a.clientY), c
    }, e.prototype.isNumeric = function (a) {
        return !isNaN(parseFloat(a))
    }, e.prototype.difference = function (a, b) {
        return {x: a.x - b.x, y: a.y - b.y}
    }, a.fn.owlCarousel = function (b) {
        var c = Array.prototype.slice.call(arguments, 1);
        return this.each(function () {
            var d = a(this), f = d.data("owl.carousel");
            f || (f = new e(this, "object" == typeof b && b), d.data("owl.carousel", f), a.each(["next", "prev", "to", "destroy", "refresh", "replace", "add", "remove"], function (b, c) {
                f.register({
                    type: e.Type.Event,
                    name: c
                }), f.$element.on(c + ".owl.carousel.core", a.proxy(function (a) {
                    a.namespace && a.relatedTarget !== this && (this.suppress([c]), f[c].apply(this, [].slice.call(arguments, 1)), this.release([c]))
                }, f))
            })), "string" == typeof b && "_" !== b.charAt(0) && f[b].apply(f, c)
        })
    }, a.fn.owlCarousel.Constructor = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (b) {
        this._core = b, this._interval = null, this._visible = null, this._handlers = {
            "initialized.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.autoRefresh && this.watch()
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers)
    };
    e.Defaults = {autoRefresh: !0, autoRefreshInterval: 500}, e.prototype.watch = function () {
        this._interval || (this._visible = this._core.isVisible(), this._interval = b.setInterval(a.proxy(this.refresh, this), this._core.settings.autoRefreshInterval))
    }, e.prototype.refresh = function () {
        this._core.isVisible() !== this._visible && (this._visible = !this._visible, this._core.$element.toggleClass("owl-hidden", !this._visible), this._visible && this._core.invalidate("width") && this._core.refresh())
    }, e.prototype.destroy = function () {
        var a, c;
        b.clearInterval(this._interval);
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (c in Object.getOwnPropertyNames(this)) "function" != typeof this[c] && (this[c] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.AutoRefresh = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (b) {
        this._core = b, this._loaded = [], this._handlers = {
            "initialized.owl.carousel change.owl.carousel resized.owl.carousel": a.proxy(function (b) {
                if (b.namespace && this._core.settings && this._core.settings.lazyLoad && (b.property && "position" == b.property.name || "initialized" == b.type)) {
                    var c = this._core.settings, e = c.center && Math.ceil(c.items / 2) || c.items,
                        f = c.center && -1 * e || 0,
                        g = (b.property && b.property.value !== d ? b.property.value : this._core.current()) + f,
                        h = this._core.clones().length, i = a.proxy(function (a, b) {
                            this.load(b)
                        }, this);
                    for (c.lazyLoadEager > 0 && (e += c.lazyLoadEager, c.loop && (g -= c.lazyLoadEager, e++)); f++ < e;) this.load(h / 2 + this._core.relative(g)), h && a.each(this._core.clones(this._core.relative(g)), i), g++
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers)
    };
    e.Defaults = {lazyLoad: !1, lazyLoadEager: 0}, e.prototype.load = function (c) {
        var d = this._core.$stage.children().eq(c), e = d && d.find(".owl-lazy");
        !e || a.inArray(d.get(0), this._loaded) > -1 || (e.each(a.proxy(function (c, d) {
            var e, f = a(d),
                g = b.devicePixelRatio > 1 && f.attr("data-src-retina") || f.attr("data-src") || f.attr("data-srcset");
            this._core.trigger("load", {
                element: f,
                url: g
            }, "lazy"), f.is("img") ? f.one("load.owl.lazy", a.proxy(function () {
                f.css("opacity", 1), this._core.trigger("loaded", {element: f, url: g}, "lazy")
            }, this)).attr("src", g) : f.is("source") ? f.one("load.owl.lazy", a.proxy(function () {
                this._core.trigger("loaded", {element: f, url: g}, "lazy")
            }, this)).attr("srcset", g) : (e = new Image, e.onload = a.proxy(function () {
                f.css({"background-image": 'url("' + g + '")', opacity: "1"}), this._core.trigger("loaded", {
                    element: f,
                    url: g
                }, "lazy")
            }, this), e.src = g)
        }, this)), this._loaded.push(d.get(0)))
    }, e.prototype.destroy = function () {
        var a, b;
        for (a in this.handlers) this._core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Lazy = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (c) {
        this._core = c, this._previousHeight = null, this._handlers = {
            "initialized.owl.carousel refreshed.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.autoHeight && this.update()
            }, this), "changed.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.autoHeight && "position" === a.property.name && this.update()
            }, this), "loaded.owl.lazy": a.proxy(function (a) {
                a.namespace && this._core.settings.autoHeight && a.element.closest("." + this._core.settings.itemClass).index() === this._core.current() && this.update()
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers), this._intervalId = null;
        var d = this;
        a(b).on("load", function () {
            d._core.settings.autoHeight && d.update()
        }), a(b).resize(function () {
            d._core.settings.autoHeight && (null != d._intervalId && clearTimeout(d._intervalId), d._intervalId = setTimeout(function () {
                d.update()
            }, 250))
        })
    };
    e.Defaults = {autoHeight: !1, autoHeightClass: "owl-height"}, e.prototype.update = function () {
        var b = this._core._current, c = b + this._core.settings.items, d = this._core.settings.lazyLoad,
            e = this._core.$stage.children().toArray().slice(b, c), f = [], g = 0;
        a.each(e, function (b, c) {
            f.push(a(c).height())
        }), g = Math.max.apply(null, f), g <= 1 && d && this._previousHeight && (g = this._previousHeight), this._previousHeight = g, this._core.$stage.parent().height(g).addClass(this._core.settings.autoHeightClass)
    }, e.prototype.destroy = function () {
        var a, b;
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.AutoHeight = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (b) {
        this._core = b, this._videos = {}, this._playing = null, this._handlers = {
            "initialized.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.register({type: "state", name: "playing", tags: ["interacting"]})
            }, this), "resize.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.video && this.isInFullScreen() && a.preventDefault()
            }, this), "refreshed.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.is("resizing") && this._core.$stage.find(".cloned .owl-video-frame").remove()
            }, this), "changed.owl.carousel": a.proxy(function (a) {
                a.namespace && "position" === a.property.name && this._playing && this.stop()
            }, this), "prepared.owl.carousel": a.proxy(function (b) {
                if (b.namespace) {
                    var c = a(b.content).find(".owl-video");
                    c.length && (c.css("display", "none"), this.fetch(c, a(b.content)))
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers), this._core.$element.on("click.owl.video", ".owl-video-play-icon", a.proxy(function (a) {
            this.play(a)
        }, this))
    };
    e.Defaults = {video: !1, videoHeight: !1, videoWidth: !1}, e.prototype.fetch = function (a, b) {
        var c = function () {
                return a.attr("data-vimeo-id") ? "vimeo" : a.attr("data-vzaar-id") ? "vzaar" : "youtube"
            }(), d = a.attr("data-vimeo-id") || a.attr("data-youtube-id") || a.attr("data-vzaar-id"),
            e = a.attr("data-width") || this._core.settings.videoWidth,
            f = a.attr("data-height") || this._core.settings.videoHeight, g = a.attr("href");
        if (!g) throw new Error("Missing video URL.");
        if (d = g.match(/(http:|https:|)\/\/(player.|www.|app.)?(vimeo\.com|youtu(be\.com|\.be|be\.googleapis\.com|be\-nocookie\.com)|vzaar\.com)\/(video\/|videos\/|embed\/|channels\/.+\/|groups\/.+\/|watch\?v=|v\/)?([A-Za-z0-9._%-]*)(\&\S+)?/), d[3].indexOf("youtu") > -1) c = "youtube"; else if (d[3].indexOf("vimeo") > -1) c = "vimeo"; else {
            if (!(d[3].indexOf("vzaar") > -1)) throw new Error("Video URL not supported.");
            c = "vzaar"
        }
        d = d[6], this._videos[g] = {
            type: c,
            id: d,
            width: e,
            height: f
        }, b.attr("data-video", g), this.thumbnail(a, this._videos[g])
    }, e.prototype.thumbnail = function (b, c) {
        var d, e, f, g = c.width && c.height ? "width:" + c.width + "px;height:" + c.height + "px;" : "",
            h = b.find("img"), i = "src", j = "", k = this._core.settings, l = function (c) {
                e = '<div class="owl-video-play-icon"></div>', d = k.lazyLoad ? a("<div/>", {
                    class: "owl-video-tn " + j,
                    srcType: c
                }) : a("<div/>", {
                    class: "owl-video-tn",
                    style: "opacity:1;background-image:url(" + c + ")"
                }), b.after(d), b.after(e)
            };
        if (b.wrap(a("<div/>", {
            class: "owl-video-wrapper",
            style: g
        })), this._core.settings.lazyLoad && (i = "data-src", j = "owl-lazy"), h.length) return l(h.attr(i)), h.remove(), !1;
        "youtube" === c.type ? (f = "//img.youtube.com/vi/" + c.id + "/hqdefault.jpg", l(f)) : "vimeo" === c.type ? a.ajax({
            type: "GET",
            url: "//vimeo.com/api/v2/video/" + c.id + ".json",
            jsonp: "callback",
            dataType: "jsonp",
            success: function (a) {
                f = a[0].thumbnail_large, l(f)
            }
        }) : "vzaar" === c.type && a.ajax({
            type: "GET",
            url: "//vzaar.com/api/videos/" + c.id + ".json",
            jsonp: "callback",
            dataType: "jsonp",
            success: function (a) {
                f = a.framegrab_url, l(f)
            }
        })
    }, e.prototype.stop = function () {
        this._core.trigger("stop", null, "video"), this._playing.find(".owl-video-frame").remove(), this._playing.removeClass("owl-video-playing"), this._playing = null, this._core.leave("playing"), this._core.trigger("stopped", null, "video")
    }, e.prototype.play = function (b) {
        var c, d = a(b.target), e = d.closest("." + this._core.settings.itemClass),
            f = this._videos[e.attr("data-video")], g = f.width || "100%", h = f.height || this._core.$stage.height();
        this._playing || (this._core.enter("playing"), this._core.trigger("play", null, "video"), e = this._core.items(this._core.relative(e.index())), this._core.reset(e.index()), c = a('<iframe frameborder="0" allowfullscreen mozallowfullscreen webkitAllowFullScreen ></iframe>'), c.attr("height", h), c.attr("width", g), "youtube" === f.type ? c.attr("src", "//www.youtube.com/embed/" + f.id + "?autoplay=1&rel=0&v=" + f.id) : "vimeo" === f.type ? c.attr("src", "//player.vimeo.com/video/" + f.id + "?autoplay=1") : "vzaar" === f.type && c.attr("src", "//view.vzaar.com/" + f.id + "/player?autoplay=true"), a(c).wrap('<div class="owl-video-frame" />').insertAfter(e.find(".owl-video")), this._playing = e.addClass("owl-video-playing"))
    }, e.prototype.isInFullScreen = function () {
        var b = c.fullscreenElement || c.mozFullScreenElement || c.webkitFullscreenElement;
        return b && a(b).parent().hasClass("owl-video-frame")
    }, e.prototype.destroy = function () {
        var a, b;
        this._core.$element.off("click.owl.video");
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Video = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (b) {
        this.core = b, this.core.options = a.extend({}, e.Defaults, this.core.options), this.swapping = !0, this.previous = d, this.next = d, this.handlers = {
            "change.owl.carousel": a.proxy(function (a) {
                a.namespace && "position" == a.property.name && (this.previous = this.core.current(), this.next = a.property.value)
            }, this), "drag.owl.carousel dragged.owl.carousel translated.owl.carousel": a.proxy(function (a) {
                a.namespace && (this.swapping = "translated" == a.type)
            }, this), "translate.owl.carousel": a.proxy(function (a) {
                a.namespace && this.swapping && (this.core.options.animateOut || this.core.options.animateIn) && this.swap()
            }, this)
        }, this.core.$element.on(this.handlers)
    };
    e.Defaults = {
        animateOut: !1,
        animateIn: !1
    }, e.prototype.swap = function () {
        if (1 === this.core.settings.items && a.support.animation && a.support.transition) {
            this.core.speed(0);
            var b, c = a.proxy(this.clear, this), d = this.core.$stage.children().eq(this.previous),
                e = this.core.$stage.children().eq(this.next), f = this.core.settings.animateIn,
                g = this.core.settings.animateOut;
            this.core.current() !== this.previous && (g && (b = this.core.coordinates(this.previous) - this.core.coordinates(this.next), d.one(a.support.animation.end, c).css({left: b + "px"}).addClass("animated owl-animated-out").addClass(g)), f && e.one(a.support.animation.end, c).addClass("animated owl-animated-in").addClass(f))
        }
    }, e.prototype.clear = function (b) {
        a(b.target).css({left: ""}).removeClass("animated owl-animated-out owl-animated-in").removeClass(this.core.settings.animateIn).removeClass(this.core.settings.animateOut), this.core.onTransitionEnd()
    }, e.prototype.destroy = function () {
        var a, b;
        for (a in this.handlers) this.core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Animate = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    var e = function (b) {
        this._core = b, this._call = null, this._time = 0, this._timeout = 0, this._paused = !0, this._handlers = {
            "changed.owl.carousel": a.proxy(function (a) {
                a.namespace && "settings" === a.property.name ? this._core.settings.autoplay ? this.play() : this.stop() : a.namespace && "position" === a.property.name && this._paused && (this._time = 0)
            }, this), "initialized.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.autoplay && this.play()
            }, this), "play.owl.autoplay": a.proxy(function (a, b, c) {
                a.namespace && this.play(b, c)
            }, this), "stop.owl.autoplay": a.proxy(function (a) {
                a.namespace && this.stop()
            }, this), "mouseover.owl.autoplay": a.proxy(function () {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.pause()
            }, this), "mouseleave.owl.autoplay": a.proxy(function () {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.play()
            }, this), "touchstart.owl.core": a.proxy(function () {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.pause()
            }, this), "touchend.owl.core": a.proxy(function () {
                this._core.settings.autoplayHoverPause && this.play()
            }, this)
        }, this._core.$element.on(this._handlers), this._core.options = a.extend({}, e.Defaults, this._core.options)
    };
    e.Defaults = {
        autoplay: !1,
        autoplayTimeout: 5e3,
        autoplayHoverPause: !1,
        autoplaySpeed: !1
    }, e.prototype._next = function (d) {
        this._call = b.setTimeout(a.proxy(this._next, this, d), this._timeout * (Math.round(this.read() / this._timeout) + 1) - this.read()), this._core.is("interacting") || c.hidden || this._core.next(d || this._core.settings.autoplaySpeed)
    }, e.prototype.read = function () {
        return (new Date).getTime() - this._time
    }, e.prototype.play = function (c, d) {
        var e;
        this._core.is("rotating") || this._core.enter("rotating"), c = c || this._core.settings.autoplayTimeout, e = Math.min(this._time % (this._timeout || c), c), this._paused ? (this._time = this.read(), this._paused = !1) : b.clearTimeout(this._call), this._time += this.read() % c - e, this._timeout = c, this._call = b.setTimeout(a.proxy(this._next, this, d), c - e)
    }, e.prototype.stop = function () {
        this._core.is("rotating") && (this._time = 0, this._paused = !0, b.clearTimeout(this._call), this._core.leave("rotating"))
    }, e.prototype.pause = function () {
        this._core.is("rotating") && !this._paused && (this._time = this.read(), this._paused = !0, b.clearTimeout(this._call))
    }, e.prototype.destroy = function () {
        var a, b;
        this.stop();
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.autoplay = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    "use strict";
    var e = function (b) {
        this._core = b, this._initialized = !1, this._pages = [], this._controls = {}, this._templates = [], this.$element = this._core.$element, this._overrides = {
            next: this._core.next,
            prev: this._core.prev,
            to: this._core.to
        }, this._handlers = {
            "prepared.owl.carousel": a.proxy(function (b) {
                b.namespace && this._core.settings.dotsData && this._templates.push('<div class="' + this._core.settings.dotClass + '">' + a(b.content).find("[data-dot]").addBack("[data-dot]").attr("data-dot") + "</div>")
            }, this), "added.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.dotsData && this._templates.splice(a.position, 0, this._templates.pop())
            }, this), "remove.owl.carousel": a.proxy(function (a) {
                a.namespace && this._core.settings.dotsData && this._templates.splice(a.position, 1)
            }, this), "changed.owl.carousel": a.proxy(function (a) {
                a.namespace && "position" == a.property.name && this.draw()
            }, this), "initialized.owl.carousel": a.proxy(function (a) {
                a.namespace && !this._initialized && (this._core.trigger("initialize", null, "navigation"), this.initialize(), this.update(), this.draw(), this._initialized = !0, this._core.trigger("initialized", null, "navigation"))
            }, this), "refreshed.owl.carousel": a.proxy(function (a) {
                a.namespace && this._initialized && (this._core.trigger("refresh", null, "navigation"), this.update(), this.draw(), this._core.trigger("refreshed", null, "navigation"))
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this.$element.on(this._handlers)
    };
    e.Defaults = {
        nav: !1,
        navText: ['<span aria-label="Previous">&#x2039;</span>', '<span aria-label="Next">&#x203a;</span>'],
        navSpeed: !1,
        navElement: 'button type="button" role="presentation"',
        navContainer: !1,
        navContainerClass: "owl-nav",
        navClass: ["owl-prev", "owl-next"],
        slideBy: 1,
        dotClass: "owl-dot",
        dotsClass: "owl-dots",
        dots: !0,
        dotsEach: !1,
        dotsData: !1,
        dotsSpeed: !1,
        dotsContainer: !1
    }, e.prototype.initialize = function () {
        var b, c = this._core.settings;
        this._controls.$relative = (c.navContainer ? a(c.navContainer) : a("<div>").addClass(c.navContainerClass).appendTo(this.$element)).addClass("disabled"), this._controls.$previous = a("<" + c.navElement + ">").addClass(c.navClass[0]).html(c.navText[0]).prependTo(this._controls.$relative).on("click", a.proxy(function (a) {
            this.prev(c.navSpeed)
        }, this)), this._controls.$next = a("<" + c.navElement + ">").addClass(c.navClass[1]).html(c.navText[1]).appendTo(this._controls.$relative).on("click", a.proxy(function (a) {
            this.next(c.navSpeed)
        }, this)), c.dotsData || (this._templates = [a('<button role="button">').addClass(c.dotClass).append(a("<span>")).prop("outerHTML")]), this._controls.$absolute = (c.dotsContainer ? a(c.dotsContainer) : a("<div>").addClass(c.dotsClass).appendTo(this.$element)).addClass("disabled"), this._controls.$absolute.on("click", "button", a.proxy(function (b) {
            var d = a(b.target).parent().is(this._controls.$absolute) ? a(b.target).index() : a(b.target).parent().index();
            b.preventDefault(), this.to(d, c.dotsSpeed)
        }, this));
        for (b in this._overrides) this._core[b] = a.proxy(this[b], this)
    }, e.prototype.destroy = function () {
        var a, b, c, d, e;
        e = this._core.settings;
        for (a in this._handlers) this.$element.off(a, this._handlers[a]);
        for (b in this._controls) "$relative" === b && e.navContainer ? this._controls[b].html("") : this._controls[b].remove();
        for (d in this.overides) this._core[d] = this._overrides[d];
        for (c in Object.getOwnPropertyNames(this)) "function" != typeof this[c] && (this[c] = null)
    }, e.prototype.update = function () {
        var a, b, c, d = this._core.clones().length / 2, e = d + this._core.items().length, f = this._core.maximum(!0),
            g = this._core.settings, h = g.center || g.autoWidth || g.dotsData ? 1 : g.dotsEach || g.items;
        if ("page" !== g.slideBy && (g.slideBy = Math.min(g.slideBy, g.items)), g.dots || "page" == g.slideBy) for (this._pages = [], a = d, b = 0, c = 0; a < e; a++) {
            if (b >= h || 0 === b) {
                if (this._pages.push({start: Math.min(f, a - d), end: a - d + h - 1}), Math.min(f, a - d) === f) break;
                b = 0, ++c
            }
            b += this._core.mergers(this._core.relative(a))
        }
    }, e.prototype.draw = function () {
        var b, c = this._core.settings, d = this._core.items().length <= c.items,
            e = this._core.relative(this._core.current()), f = c.loop || c.rewind;
        this._controls.$relative.toggleClass("disabled", !c.nav || d), c.nav && (this._controls.$previous.toggleClass("disabled", !f && e <= this._core.minimum(!0)), this._controls.$next.toggleClass("disabled", !f && e >= this._core.maximum(!0))), this._controls.$absolute.toggleClass("disabled", !c.dots || d), c.dots && (b = this._pages.length - this._controls.$absolute.children().length, c.dotsData && 0 !== b ? this._controls.$absolute.html(this._templates.join("")) : b > 0 ? this._controls.$absolute.append(new Array(b + 1).join(this._templates[0])) : b < 0 && this._controls.$absolute.children().slice(b).remove(), this._controls.$absolute.find(".active").removeClass("active"), this._controls.$absolute.children().eq(a.inArray(this.current(), this._pages)).addClass("active"))
    }, e.prototype.onTrigger = function (b) {
        var c = this._core.settings;
        b.page = {
            index: a.inArray(this.current(), this._pages),
            count: this._pages.length,
            size: c && (c.center || c.autoWidth || c.dotsData ? 1 : c.dotsEach || c.items)
        }
    }, e.prototype.current = function () {
        var b = this._core.relative(this._core.current());
        return a.grep(this._pages, a.proxy(function (a, c) {
            return a.start <= b && a.end >= b
        }, this)).pop()
    }, e.prototype.getPosition = function (b) {
        var c, d, e = this._core.settings;
        return "page" == e.slideBy ? (c = a.inArray(this.current(), this._pages), d = this._pages.length, b ? ++c : --c, c = this._pages[(c % d + d) % d].start) : (c = this._core.relative(this._core.current()), d = this._core.items().length, b ? c += e.slideBy : c -= e.slideBy), c
    }, e.prototype.next = function (b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!0), b)
    }, e.prototype.prev = function (b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!1), b)
    }, e.prototype.to = function (b, c, d) {
        var e;
        !d && this._pages.length ? (e = this._pages.length, a.proxy(this._overrides.to, this._core)(this._pages[(b % e + e) % e].start, c)) : a.proxy(this._overrides.to, this._core)(b, c)
    }, a.fn.owlCarousel.Constructor.Plugins.Navigation = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    "use strict";
    var e = function (c) {
        this._core = c, this._hashes = {}, this.$element = this._core.$element, this._handlers = {
            "initialized.owl.carousel": a.proxy(function (c) {
                c.namespace && "URLHash" === this._core.settings.startPosition && a(b).trigger("hashchange.owl.navigation")
            }, this), "prepared.owl.carousel": a.proxy(function (b) {
                if (b.namespace) {
                    var c = a(b.content).find("[data-hash]").addBack("[data-hash]").attr("data-hash");
                    if (!c) return;
                    this._hashes[c] = b.content
                }
            }, this), "changed.owl.carousel": a.proxy(function (c) {
                if (c.namespace && "position" === c.property.name) {
                    var d = this._core.items(this._core.relative(this._core.current())),
                        e = a.map(this._hashes, function (a, b) {
                            return a === d ? b : null
                        }).join();
                    if (!e || b.location.hash.slice(1) === e) return;
                    b.location.hash = e
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this.$element.on(this._handlers), a(b).on("hashchange.owl.navigation", a.proxy(function (a) {
            var c = b.location.hash.substring(1), e = this._core.$stage.children(),
                f = this._hashes[c] && e.index(this._hashes[c]);
            f !== d && f !== this._core.current() && this._core.to(this._core.relative(f), !1, !0)
        }, this))
    };
    e.Defaults = {URLhashListener: !1}, e.prototype.destroy = function () {
        var c, d;
        a(b).off("hashchange.owl.navigation");
        for (c in this._handlers) this._core.$element.off(c, this._handlers[c]);
        for (d in Object.getOwnPropertyNames(this)) "function" != typeof this[d] && (this[d] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Hash = e
}(window.Zepto || window.jQuery, window, document), function (a, b, c, d) {
    function e(b, c) {
        var e = !1, f = b.charAt(0).toUpperCase() + b.slice(1);
        return a.each((b + " " + h.join(f + " ") + f).split(" "), function (a, b) {
            if (g[b] !== d) return e = !c || b, !1
        }), e
    }

    function f(a) {
        return e(a, !0)
    }

    var g = a("<support>").get(0).style, h = "Webkit Moz O ms".split(" "), i = {
        transition: {
            end: {
                WebkitTransition: "webkitTransitionEnd",
                MozTransition: "transitionend",
                OTransition: "oTransitionEnd",
                transition: "transitionend"
            }
        },
        animation: {
            end: {
                WebkitAnimation: "webkitAnimationEnd",
                MozAnimation: "animationend",
                OAnimation: "oAnimationEnd",
                animation: "animationend"
            }
        }
    }, j = {
        csstransforms: function () {
            return !!e("transform")
        }, csstransforms3d: function () {
            return !!e("perspective")
        }, csstransitions: function () {
            return !!e("transition")
        }, cssanimations: function () {
            return !!e("animation")
        }
    };
    j.csstransitions() && (a.support.transition = new String(f("transition")), a.support.transition.end = i.transition.end[a.support.transition]), j.cssanimations() && (a.support.animation = new String(f("animation")), a.support.animation.end = i.animation.end[a.support.animation]), j.csstransforms() && (a.support.transform = new String(f("transform")), a.support.transform3d = j.csstransforms3d())
}(window.Zepto || window.jQuery, window, document);/*! WOW - v1.0.1 - 2014-08-15
* Copyright (c) 2014 Matthieu Aussaguel; Licensed MIT */
(function () {
    var a, b, c, d = function (a, b) {
        return function () {
            return a.apply(b, arguments)
        }
    }, e = [].indexOf || function (a) {
        for (var b = 0, c = this.length; c > b; b++) if (b in this && this[b] === a) return b;
        return -1
    };
    b = function () {
        function a() {
        }

        return a.prototype.extend = function (a, b) {
            var c, d;
            for (c in b) d = b[c], null == a[c] && (a[c] = d);
            return a
        }, a.prototype.isMobile = function (a) {
            return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(a)
        }, a
    }(), c = this.WeakMap || this.MozWeakMap || (c = function () {
        function a() {
            this.keys = [], this.values = []
        }

        return a.prototype.get = function (a) {
            var b, c, d, e, f;
            for (f = this.keys, b = d = 0, e = f.length; e > d; b = ++d) if (c = f[b], c === a) return this.values[b]
        }, a.prototype.set = function (a, b) {
            var c, d, e, f, g;
            for (g = this.keys, c = e = 0, f = g.length; f > e; c = ++e) if (d = g[c], d === a) return void (this.values[c] = b);
            return this.keys.push(a), this.values.push(b)
        }, a
    }()), a = this.MutationObserver || this.WebkitMutationObserver || this.MozMutationObserver || (a = function () {
        function a() {
            console.warn("MutationObserver is not supported by your browser."), console.warn("WOW.js cannot detect dom mutations, please call .sync() after loading new content.")
        }

        return a.notSupported = !0, a.prototype.observe = function () {
        }, a
    }()), this.WOW = function () {
        function f(a) {
            null == a && (a = {}), this.scrollCallback = d(this.scrollCallback, this), this.scrollHandler = d(this.scrollHandler, this), this.start = d(this.start, this), this.scrolled = !0, this.config = this.util().extend(a, this.defaults), this.animationNameCache = new c
        }

        return f.prototype.defaults = {
            boxClass: "wow",
            animateClass: "animated",
            offset: 0,
            mobile: !0,
            live: !0
        }, f.prototype.init = function () {
            var a;
            return this.element = window.document.documentElement, "interactive" === (a = document.readyState) || "complete" === a ? this.start() : document.addEventListener("DOMContentLoaded", this.start), this.finished = []
        }, f.prototype.start = function () {
            var b, c, d, e;
            if (this.stopped = !1, this.boxes = function () {
                var a, c, d, e;
                for (d = this.element.querySelectorAll("." + this.config.boxClass), e = [], a = 0, c = d.length; c > a; a++) b = d[a], e.push(b);
                return e
            }.call(this), this.all = function () {
                var a, c, d, e;
                for (d = this.boxes, e = [], a = 0, c = d.length; c > a; a++) b = d[a], e.push(b);
                return e
            }.call(this), this.boxes.length) if (this.disabled()) this.resetStyle(); else {
                for (e = this.boxes, c = 0, d = e.length; d > c; c++) b = e[c], this.applyStyle(b, !0);
                window.addEventListener("scroll", this.scrollHandler, !1), window.addEventListener("resize", this.scrollHandler, !1), this.interval = setInterval(this.scrollCallback, 50)
            }
            return this.config.live ? new a(function (a) {
                return function (b) {
                    var c, d, e, f, g;
                    for (g = [], e = 0, f = b.length; f > e; e++) d = b[e], g.push(function () {
                        var a, b, e, f;
                        for (e = d.addedNodes || [], f = [], a = 0, b = e.length; b > a; a++) c = e[a], f.push(this.doSync(c));
                        return f
                    }.call(a));
                    return g
                }
            }(this)).observe(document.body, {childList: !0, subtree: !0}) : void 0
        }, f.prototype.stop = function () {
            return this.stopped = !0, window.removeEventListener("scroll", this.scrollHandler, !1), window.removeEventListener("resize", this.scrollHandler, !1), null != this.interval ? clearInterval(this.interval) : void 0
        }, f.prototype.sync = function () {
            return a.notSupported ? this.doSync(this.element) : void 0
        }, f.prototype.doSync = function (a) {
            var b, c, d, f, g;
            if (!this.stopped) {
                if (null == a && (a = this.element), 1 !== a.nodeType) return;
                for (a = a.parentNode || a, f = a.querySelectorAll("." + this.config.boxClass), g = [], c = 0, d = f.length; d > c; c++) b = f[c], e.call(this.all, b) < 0 ? (this.applyStyle(b, !0), this.boxes.push(b), this.all.push(b), g.push(this.scrolled = !0)) : g.push(void 0);
                return g
            }
        }, f.prototype.show = function (a) {
            return this.applyStyle(a), a.className = "" + a.className + " " + this.config.animateClass
        }, f.prototype.applyStyle = function (a, b) {
            var c, d, e;
            return d = a.getAttribute("data-wow-duration"), c = a.getAttribute("data-wow-delay"), e = a.getAttribute("data-wow-iteration"), this.animate(function (f) {
                return function () {
                    return f.customStyle(a, b, d, c, e)
                }
            }(this))
        }, f.prototype.animate = function () {
            return "requestAnimationFrame" in window ? function (a) {
                return window.requestAnimationFrame(a)
            } : function (a) {
                return a()
            }
        }(), f.prototype.resetStyle = function () {
            var a, b, c, d, e;
            for (d = this.boxes, e = [], b = 0, c = d.length; c > b; b++) a = d[b], e.push(a.setAttribute("style", "visibility: visible;"));
            return e
        }, f.prototype.customStyle = function (a, b, c, d, e) {
            return b && this.cacheAnimationName(a), a.style.visibility = b ? "hidden" : "visible", c && this.vendorSet(a.style, {animationDuration: c}), d && this.vendorSet(a.style, {animationDelay: d}), e && this.vendorSet(a.style, {animationIterationCount: e}), this.vendorSet(a.style, {animationName: b ? "none" : this.cachedAnimationName(a)}), a
        }, f.prototype.vendors = ["moz", "webkit"], f.prototype.vendorSet = function (a, b) {
            var c, d, e, f;
            f = [];
            for (c in b) d = b[c], a["" + c] = d, f.push(function () {
                var b, f, g, h;
                for (g = this.vendors, h = [], b = 0, f = g.length; f > b; b++) e = g[b], h.push(a["" + e + c.charAt(0).toUpperCase() + c.substr(1)] = d);
                return h
            }.call(this));
            return f
        }, f.prototype.vendorCSS = function (a, b) {
            var c, d, e, f, g, h;
            for (d = window.getComputedStyle(a), c = d.getPropertyCSSValue(b), h = this.vendors, f = 0, g = h.length; g > f; f++) e = h[f], c = c || d.getPropertyCSSValue("-" + e + "-" + b);
            return c
        }, f.prototype.animationName = function (a) {
            var b;
            try {
                b = this.vendorCSS(a, "animation-name").cssText
            } catch (c) {
                b = window.getComputedStyle(a).getPropertyValue("animation-name")
            }
            return "none" === b ? "" : b
        }, f.prototype.cacheAnimationName = function (a) {
            return this.animationNameCache.set(a, this.animationName(a))
        }, f.prototype.cachedAnimationName = function (a) {
            return this.animationNameCache.get(a)
        }, f.prototype.scrollHandler = function () {
            return this.scrolled = !0
        }, f.prototype.scrollCallback = function () {
            var a;
            return !this.scrolled || (this.scrolled = !1, this.boxes = function () {
                var b, c, d, e;
                for (d = this.boxes, e = [], b = 0, c = d.length; c > b; b++) a = d[b], a && (this.isVisible(a) ? this.show(a) : e.push(a));
                return e
            }.call(this), this.boxes.length || this.config.live) ? void 0 : this.stop()
        }, f.prototype.offsetTop = function (a) {
            for (var b; void 0 === a.offsetTop;) a = a.parentNode;
            for (b = a.offsetTop; a = a.offsetParent;) b += a.offsetTop;
            return b
        }, f.prototype.isVisible = function (a) {
            var b, c, d, e, f;
            return c = a.getAttribute("data-wow-offset") || this.config.offset, f = window.pageYOffset, e = f + Math.min(this.element.clientHeight, innerHeight) - c, d = this.offsetTop(a), b = d + a.clientHeight, e >= d && b >= f
        }, f.prototype.util = function () {
            return null != this._util ? this._util : this._util = new b
        }, f.prototype.disabled = function () {
            return !this.config.mobile && this.util().isMobile(navigator.userAgent)
        }, f
    }()
}).call(this);/*!
 * GSAP 3.10.4
 * https://greensock.com
 * 
 * @license Copyright 2022, GreenSock. All rights reserved.
 * Subject to the terms at https://greensock.com/standard-license or for Club GreenSock members, the agreement issued with that membership.
 * @author: Jack Doyle, jack@greensock.com
 */
!function (t, e) {
    "object" == typeof exports && "undefined" != typeof module ? e(exports) : "function" == typeof define && define.amd ? define(["exports"], e) : e((t = t || self).window = t.window || {})
}(this, (function (t) {
    "use strict";

    function e(t, e) {
        t.prototype = Object.create(e.prototype), (t.prototype.constructor = t).__proto__ = e
    }

    function r(t) {
        if (void 0 === t) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
        return t
    }

    function i(t) {
        return "string" == typeof t
    }

    function n(t) {
        return "function" == typeof t
    }

    function s(t) {
        return "number" == typeof t
    }

    function a(t) {
        return void 0 === t
    }

    function o(t) {
        return "object" == typeof t
    }

    function u(t) {
        return !1 !== t
    }

    function h() {
        return "undefined" != typeof window
    }

    function f(t) {
        return n(t) || i(t)
    }

    function l(t) {
        return (Tt = de(t, ae)) && gr
    }

    function p(t, e) {
        return console.warn("Invalid property", t, "set to", e, "Missing plugin? gsap.registerPlugin()")
    }

    function _(t, e) {
        return !e && console.warn(t)
    }

    function c(t, e) {
        return t && (ae[t] = e) && Tt && (Tt[t] = e) || ae
    }

    function d() {
        return 0
    }

    function m(t) {
        var e, r, i = t[0];
        if (o(i) || n(i) || (t = [t]), !(e = (i._gsap || {}).harness)) {
            for (r = _e.length; r-- && !_e[r].targetTest(i);) ;
            e = _e[r]
        }
        for (r = t.length; r--;) t[r] && (t[r]._gsap || (t[r]._gsap = new Ye(t[r], e))) || t.splice(r, 1);
        return t
    }

    function g(t) {
        return t._gsap || m(we(t))[0]._gsap
    }

    function v(t, e, r) {
        return (r = t[e]) && n(r) ? t[e]() : a(r) && t.getAttribute && t.getAttribute(e) || r
    }

    function y(t, e) {
        return (t = t.split(",")).forEach(e) || t
    }

    function T(t) {
        return Math.round(1e5 * t) / 1e5 || 0
    }

    function x(t) {
        return Math.round(1e7 * t) / 1e7 || 0
    }

    function w(t, e) {
        var r = e.charAt(0), i = parseFloat(e.substr(2));
        return t = parseFloat(t), "+" === r ? t + i : "-" === r ? t - i : "*" === r ? t * i : t / i
    }

    function b(t, e) {
        for (var r = e.length, i = 0; t.indexOf(e[i]) < 0 && ++i < r;) ;
        return i < r
    }

    function M() {
        var t, e, r = ue.length, i = ue.slice(0);
        for (he = {}, t = ue.length = 0; t < r; t++) (e = i[t]) && e._lazy && (e.render(e._lazy[0], e._lazy[1], !0)._lazy = 0)
    }

    function O(t, e, r, i) {
        ue.length && M(), t.render(e, r, i), ue.length && M()
    }

    function k(t) {
        var e = parseFloat(t);
        return (e || 0 === e) && (t + "").match(ne).length < 2 ? e : i(t) ? t.trim() : t
    }

    function A(t) {
        return t
    }

    function C(t, e) {
        for (var r in e) r in t || (t[r] = e[r]);
        return t
    }

    function D(t, e) {
        for (var r in e) "__proto__" !== r && "constructor" !== r && "prototype" !== r && (t[r] = o(e[r]) ? D(t[r] || (t[r] = {}), e[r]) : e[r]);
        return t
    }

    function P(t, e) {
        var r, i = {};
        for (r in t) r in e || (i[r] = t[r]);
        return i
    }

    function S(t) {
        var e = t.parent || mt, r = t.keyframes ? function (t) {
            return function (e, r) {
                for (var i in r) i in e || "duration" === i && t || "ease" === i || (e[i] = r[i])
            }
        }(Jt(t.keyframes)) : C;
        if (u(t.inherit)) for (; e;) r(t, e.vars.defaults), e = e.parent || e._dp;
        return t
    }

    function z(t, e, r, i, n) {
        void 0 === r && (r = "_first"), void 0 === i && (i = "_last");
        var s, a = t[i];
        if (n) for (s = e[n]; a && a[n] > s;) a = a._prev;
        return a ? (e._next = a._next, a._next = e) : (e._next = t[r], t[r] = e), e._next ? e._next._prev = e : t[i] = e, e._prev = a, e.parent = e._dp = t, e
    }

    function R(t, e, r, i) {
        void 0 === r && (r = "_first"), void 0 === i && (i = "_last");
        var n = e._prev, s = e._next;
        n ? n._next = s : t[r] === e && (t[r] = s), s ? s._prev = n : t[i] === e && (t[i] = n), e._next = e._prev = e.parent = null
    }

    function E(t, e) {
        !t.parent || e && !t.parent.autoRemoveChildren || t.parent.remove(t), t._act = 0
    }

    function F(t, e) {
        if (t && (!e || e._end > t._dur || e._start < 0)) for (var r = t; r;) r._dirty = 1, r = r.parent;
        return t
    }

    function B(t) {
        return t._repeat ? me(t._tTime, t = t.duration() + t._rDelay) * t : 0
    }

    function I(t, e) {
        return (t - e._start) * e._ts + (0 <= e._ts ? 0 : e._dirty ? e.totalDuration() : e._tDur)
    }

    function L(t) {
        return t._end = x(t._start + (t._tDur / Math.abs(t._ts || t._rts || Vt) || 0))
    }

    function Y(t, e) {
        var r = t._dp;
        return r && r.smoothChildTiming && t._ts && (t._start = x(r._time - (0 < t._ts ? e / t._ts : ((t._dirty ? t.totalDuration() : t._tDur) - e) / -t._ts)), L(t), r._dirty || F(r, t)), t
    }

    function U(t, e) {
        var r;
        if ((e._time || e._initted && !e._dur) && (r = I(t.rawTime(), e), (!e._dur || Te(0, e.totalDuration(), r) - e._tTime > Vt) && e.render(r, !0)), F(t, e)._dp && t._initted && t._time >= t._dur && t._ts) {
            if (t._dur < t.duration()) for (r = t; r._dp;) 0 <= r.rawTime() && r.totalTime(r._tTime), r = r._dp;
            t._zTime = -Vt
        }
    }

    function X(t, e, r, i) {
        return e.parent && E(e), e._start = x((s(r) ? r : r || t !== mt ? ye(t, r, e) : t._time) + e._delay), e._end = x(e._start + (e.totalDuration() / Math.abs(e.timeScale()) || 0)), z(t, e, "_first", "_last", t._sort ? "_start" : 0), ge(e) || (t._recent = e), i || U(t, e), t
    }

    function N(t, e) {
        return (ae.ScrollTrigger || p("scrollTrigger", e)) && ae.ScrollTrigger.create(e, t)
    }

    function q(t, e, r, i) {
        return We(t, e), t._initted ? !r && t._pt && (t._dur && !1 !== t.vars.lazy || !t._dur && t.vars.lazy) && wt !== De.frame ? (ue.push(t), t._lazy = [e, i], 1) : void 0 : 1
    }

    function V(t, e, r, i) {
        var n = t._repeat, s = x(e) || 0, a = t._tTime / t._tDur;
        return a && !i && (t._time *= s / t._dur), t._dur = s, t._tDur = n ? n < 0 ? 1e10 : x(s * (n + 1) + t._rDelay * n) : s, 0 < a && !i ? Y(t, t._tTime = t._tDur * a) : t.parent && L(t), r || F(t.parent, t), t
    }

    function j(t) {
        return t instanceof Ne ? F(t) : V(t, t._dur)
    }

    function Q(t, e, r) {
        var i, n, a = s(e[1]), o = (a ? 2 : 1) + (t < 2 ? 0 : 1), h = e[o];
        if (a && (h.duration = e[1]), h.parent = r, t) {
            for (i = h, n = r; n && !("immediateRender" in i);) i = n.vars.defaults || {}, n = u(n.vars.inherit) && n.parent;
            h.immediateRender = u(i.immediateRender), t < 2 ? h.runBackwards = 1 : h.startAt = e[o - 1]
        }
        return new Je(e[0], h, e[1 + o])
    }

    function G(t, e) {
        return t || 0 === t ? e(t) : e
    }

    function W(t, e) {
        return i(t) && (e = se.exec(t)) ? e[1] : ""
    }

    function H(t, e) {
        return t && o(t) && "length" in t && (!e && !t.length || t.length - 1 in t && o(t[0])) && !t.nodeType && t !== gt
    }

    function Z(t) {
        return t.sort((function () {
            return .5 - Math.random()
        }))
    }

    function $(t) {
        if (n(t)) return t;
        var e = o(t) ? t : {each: t}, r = Fe(e.ease), s = e.from || 0, a = parseFloat(e.base) || 0, u = {},
            h = 0 < s && s < 1, f = isNaN(s) || h, l = e.axis, p = s, _ = s;
        return i(s) ? p = _ = {
            center: .5,
            edges: .5,
            end: 1
        }[s] || 0 : !h && f && (p = s[0], _ = s[1]), function (t, i, n) {
            var o, h, c, d, m, g, v, y, T, w = (n || e).length, b = u[w];
            if (!b) {
                if (!(T = "auto" === e.grid ? 0 : (e.grid || [1, qt])[1])) {
                    for (v = -qt; v < (v = n[T++].getBoundingClientRect().left) && T < w;) ;
                    T--
                }
                for (b = u[w] = [], o = f ? Math.min(T, w) * p - .5 : s % T, h = T === qt ? 0 : f ? w * _ / T - .5 : s / T | 0, y = qt, g = v = 0; g < w; g++) c = g % T - o, d = h - (g / T | 0), b[g] = m = l ? Math.abs("y" === l ? d : c) : Wt(c * c + d * d), v < m && (v = m), m < y && (y = m);
                "random" === s && Z(b), b.max = v - y, b.min = y, b.v = w = (parseFloat(e.amount) || parseFloat(e.each) * (w < T ? w - 1 : l ? "y" === l ? w / T : T : Math.max(T, w / T)) || 0) * ("edges" === s ? -1 : 1), b.b = w < 0 ? a - w : a, b.u = W(e.amount || e.each) || 0, r = r && w < 0 ? Ee(r) : r
            }
            return w = (b[t] - b.min) / b.max || 0, x(b.b + (r ? r(w) : w) * b.v) + b.u
        }
    }

    function J(t) {
        var e = Math.pow(10, ((t + "").split(".")[1] || "").length);
        return function (r) {
            var i = Math.round(parseFloat(r) / t) * t * e;
            return (i - i % 1) / e + (s(r) ? 0 : W(r))
        }
    }

    function K(t, e) {
        var r, i, a = Jt(t);
        return !a && o(t) && (r = a = t.radius || qt, t.values ? (t = we(t.values), (i = !s(t[0])) && (r *= r)) : t = J(t.increment)), G(e, a ? n(t) ? function (e) {
            return i = t(e), Math.abs(i - e) <= r ? i : e
        } : function (e) {
            for (var n, a, o = parseFloat(i ? e.x : e), u = parseFloat(i ? e.y : 0), h = qt, f = 0, l = t.length; l--;) (n = i ? (n = t[l].x - o) * n + (a = t[l].y - u) * a : Math.abs(t[l] - o)) < h && (h = n, f = l);
            return f = !r || h <= r ? t[f] : e, i || f === e || s(e) ? f : f + W(e)
        } : J(t))
    }

    function tt(t, e, r, i) {
        return G(Jt(t) ? !e : !0 === r ? !!(r = 0) : !i, (function () {
            return Jt(t) ? t[~~(Math.random() * t.length)] : (r = r || 1e-5) && (i = r < 1 ? Math.pow(10, (r + "").length - 2) : 1) && Math.floor(Math.round((t - r / 2 + Math.random() * (e - t + .99 * r)) / r) * r * i) / i
        }))
    }

    function et(t, e, r) {
        return G(r, (function (r) {
            return t[~~e(r)]
        }))
    }

    function rt(t) {
        for (var e, r, i, n, s = 0, a = ""; ~(e = t.indexOf("random(", s));) i = t.indexOf(")", e), n = "[" === t.charAt(e + 7), r = t.substr(e + 7, i - e - 7).match(n ? ne : Kt), a += t.substr(s, e - s) + tt(n ? r : +r[0], n ? 0 : +r[1], +r[2] || 1e-5), s = i + 1;
        return a + t.substr(s, t.length - s)
    }

    function it(t, e, r) {
        var i, n, s, a = t.labels, o = qt;
        for (i in a) (n = a[i] - e) < 0 == !!r && n && o > (n = Math.abs(n)) && (s = i, o = n);
        return s
    }

    function nt(t) {
        return E(t), t.scrollTrigger && t.scrollTrigger.kill(!1), t.progress() < 1 && Me(t, "onInterrupt"), t
    }

    function st(t, e, r) {
        return (6 * (t += t < 0 ? 1 : 1 < t ? -1 : 0) < 1 ? e + (r - e) * t * 6 : t < .5 ? r : 3 * t < 2 ? e + (r - e) * (2 / 3 - t) * 6 : e) * Oe + .5 | 0
    }

    function at(t, e, r) {
        var i, n, a, o, u, h, f, l, p, _, c = t ? s(t) ? [t >> 16, t >> 8 & Oe, t & Oe] : 0 : ke.black;
        if (!c) {
            if ("," === t.substr(-1) && (t = t.substr(0, t.length - 1)), ke[t]) c = ke[t]; else if ("#" === t.charAt(0)) {
                if (t.length < 6 && (t = "#" + (i = t.charAt(1)) + i + (n = t.charAt(2)) + n + (a = t.charAt(3)) + a + (5 === t.length ? t.charAt(4) + t.charAt(4) : "")), 9 === t.length) return [(c = parseInt(t.substr(1, 6), 16)) >> 16, c >> 8 & Oe, c & Oe, parseInt(t.substr(7), 16) / 255];
                c = [(t = parseInt(t.substr(1), 16)) >> 16, t >> 8 & Oe, t & Oe]
            } else if ("hsl" === t.substr(0, 3)) if (c = _ = t.match(Kt), e) {
                if (~t.indexOf("=")) return c = t.match(te), r && c.length < 4 && (c[3] = 1), c
            } else o = +c[0] % 360 / 360, u = c[1] / 100, i = 2 * (h = c[2] / 100) - (n = h <= .5 ? h * (u + 1) : h + u - h * u), 3 < c.length && (c[3] *= 1), c[0] = st(o + 1 / 3, i, n), c[1] = st(o, i, n), c[2] = st(o - 1 / 3, i, n); else c = t.match(Kt) || ke.transparent;
            c = c.map(Number)
        }
        return e && !_ && (i = c[0] / Oe, n = c[1] / Oe, a = c[2] / Oe, h = ((f = Math.max(i, n, a)) + (l = Math.min(i, n, a))) / 2, f === l ? o = u = 0 : (p = f - l, u = .5 < h ? p / (2 - f - l) : p / (f + l), o = f === i ? (n - a) / p + (n < a ? 6 : 0) : f === n ? (a - i) / p + 2 : (i - n) / p + 4, o *= 60), c[0] = ~~(o + .5), c[1] = ~~(100 * u + .5), c[2] = ~~(100 * h + .5)), r && c.length < 4 && (c[3] = 1), c
    }

    function ot(t) {
        var e = [], r = [], i = -1;
        return t.split(Ae).forEach((function (t) {
            var n = t.match(ee) || [];
            e.push.apply(e, n), r.push(i += n.length + 1)
        })), e.c = r, e
    }

    function ut(t, e, r) {
        var i, n, s, a, o = "", u = (t + o).match(Ae), h = e ? "hsla(" : "rgba(", f = 0;
        if (!u) return t;
        if (u = u.map((function (t) {
            return (t = at(t, e, 1)) && h + (e ? t[0] + "," + t[1] + "%," + t[2] + "%," + t[3] : t.join(",")) + ")"
        })), r && (s = ot(t), (i = r.c).join(o) !== s.c.join(o))) for (a = (n = t.replace(Ae, "1").split(ee)).length - 1; f < a; f++) o += n[f] + (~i.indexOf(f) ? u.shift() || h + "0,0,0,0)" : (s.length ? s : u.length ? u : r).shift());
        if (!n) for (a = (n = t.split(Ae)).length - 1; f < a; f++) o += n[f] + u[f];
        return o + n[a]
    }

    function ht(t) {
        var e, r = t.join(" ");
        if (Ae.lastIndex = 0, Ae.test(r)) return e = Ce.test(r), t[1] = ut(t[1], e), t[0] = ut(t[0], e, ot(t[1])), !0
    }

    function ft(t, e) {
        for (var r, i = t._first; i;) i instanceof Ne ? ft(i, e) : !i.vars.yoyoEase || i._yoyo && i._repeat || i._yoyo === e || (i.timeline ? ft(i.timeline, e) : (r = i._ease, i._ease = i._yEase, i._yEase = r, i._yoyo = e)), i = i._next
    }

    function lt(t, e, r, i) {
        void 0 === r && (r = function (t) {
            return 1 - e(1 - t)
        }), void 0 === i && (i = function (t) {
            return t < .5 ? e(2 * t) / 2 : 1 - e(2 * (1 - t)) / 2
        });
        var n, s = {easeIn: e, easeOut: r, easeInOut: i};
        return y(t, (function (t) {
            for (var e in Se[t] = ae[t] = s, Se[n = t.toLowerCase()] = r, s) Se[n + ("easeIn" === e ? ".in" : "easeOut" === e ? ".out" : ".inOut")] = Se[t + "." + e] = s[e]
        })), s
    }

    function pt(t) {
        return function (e) {
            return e < .5 ? (1 - t(1 - 2 * e)) / 2 : .5 + t(2 * (e - .5)) / 2
        }
    }

    function _t(t, e, r) {
        function i(t) {
            return 1 === t ? 1 : n * Math.pow(2, -10 * t) * Zt((t - a) * s) + 1
        }

        var n = 1 <= e ? e : 1, s = (r || (t ? .3 : .45)) / (e < 1 ? e : 1), a = s / jt * (Math.asin(1 / n) || 0),
            o = "out" === t ? i : "in" === t ? function (t) {
                return 1 - i(1 - t)
            } : pt(i);
        return s = jt / s, o.config = function (e, r) {
            return _t(t, e, r)
        }, o
    }

    function ct(t, e) {
        function r(t) {
            return t ? --t * t * ((e + 1) * t + e) + 1 : 0
        }

        void 0 === e && (e = 1.70158);
        var i = "out" === t ? r : "in" === t ? function (t) {
            return 1 - r(1 - t)
        } : pt(r);
        return i.config = function (e) {
            return ct(t, e)
        }, i
    }

    var dt, mt, gt, vt, yt, Tt, xt, wt, bt, Mt, Ot, kt, At, Ct, Dt, Pt, St, zt, Rt, Et, Ft, Bt, It, Lt, Yt, Ut,
        Xt = {autoSleep: 120, force3D: "auto", nullTargetWarn: 1, units: {lineHeight: ""}},
        Nt = {duration: .5, overwrite: !1, delay: 0}, qt = 1e8, Vt = 1 / qt, jt = 2 * Math.PI, Qt = jt / 4, Gt = 0,
        Wt = Math.sqrt, Ht = Math.cos, Zt = Math.sin,
        $t = "function" == typeof ArrayBuffer && ArrayBuffer.isView || function () {
        }, Jt = Array.isArray, Kt = /(?:-?\.?\d|\.)+/gi, te = /[-+=.]*\d+[.e\-+]*\d*[e\-+]*\d*/g,
        ee = /[-+=.]*\d+[.e-]*\d*[a-z%]*/g, re = /[-+=.]*\d+\.?\d*(?:e-|e\+)?\d*/gi, ie = /[+-]=-?[.\d]+/,
        ne = /[^,'"\[\]\s]+/gi, se = /^[+\-=e\s\d]*\d+[.\d]*([a-z]*|%)\s*$/i, ae = {}, oe = {}, ue = [], he = {},
        fe = {}, le = {}, pe = 30, _e = [], ce = "", de = function (t, e) {
            for (var r in e) t[r] = e[r];
            return t
        }, me = function (t, e) {
            var r = Math.floor(t /= e);
            return t && r === t ? r - 1 : r
        }, ge = function (t) {
            var e = t.data;
            return "isFromStart" === e || "isStart" === e
        }, ve = {_start: 0, endTime: d, totalDuration: d}, ye = function t(e, r, n) {
            var s, a, o, u = e.labels, h = e._recent || ve, f = e.duration() >= qt ? h.endTime(!1) : e._dur;
            return i(r) && (isNaN(r) || r in u) ? (a = r.charAt(0), o = "%" === r.substr(-1), s = r.indexOf("="), "<" === a || ">" === a ? (0 <= s && (r = r.replace(/=/, "")), ("<" === a ? h._start : h.endTime(0 <= h._repeat)) + (parseFloat(r.substr(1)) || 0) * (o ? (s < 0 ? h : n).totalDuration() / 100 : 1)) : s < 0 ? (r in u || (u[r] = f), u[r]) : (a = parseFloat(r.charAt(s - 1) + r.substr(s + 1)), o && n && (a = a / 100 * (Jt(n) ? n[0] : n).totalDuration()), 1 < s ? t(e, r.substr(0, s - 1), n) + a : f + a)) : null == r ? f : +r
        }, Te = function (t, e, r) {
            return r < t ? t : e < r ? e : r
        }, xe = [].slice, we = function (t, e, r) {
            return !i(t) || r || !vt && Pe() ? Jt(t) ? function (t, e, r) {
                return void 0 === r && (r = []), t.forEach((function (t) {
                    return i(t) && !e || H(t, 1) ? r.push.apply(r, we(t)) : r.push(t)
                })) || r
            }(t, r) : H(t) ? xe.call(t, 0) : t ? [t] : [] : xe.call((e || yt).querySelectorAll(t), 0)
        }, be = function (t, e, r, i, n) {
            var s = e - t, a = i - r;
            return G(n, (function (e) {
                return r + ((e - t) / s * a || 0)
            }))
        }, Me = function (t, e, r) {
            var i, n, s = t.vars, a = s[e];
            if (a) return i = s[e + "Params"], n = s.callbackScope || t, r && ue.length && M(), i ? a.apply(n, i) : a.call(n)
        }, Oe = 255, ke = {
            aqua: [0, Oe, Oe],
            lime: [0, Oe, 0],
            silver: [192, 192, 192],
            black: [0, 0, 0],
            maroon: [128, 0, 0],
            teal: [0, 128, 128],
            blue: [0, 0, Oe],
            navy: [0, 0, 128],
            white: [Oe, Oe, Oe],
            olive: [128, 128, 0],
            yellow: [Oe, Oe, 0],
            orange: [Oe, 165, 0],
            gray: [128, 128, 128],
            purple: [128, 0, 128],
            green: [0, 128, 0],
            red: [Oe, 0, 0],
            pink: [Oe, 192, 203],
            cyan: [0, Oe, Oe],
            transparent: [Oe, Oe, Oe, 0]
        }, Ae = function () {
            var t, e = "(?:\\b(?:(?:rgb|rgba|hsl|hsla)\\(.+?\\))|\\B#(?:[0-9a-f]{3,4}){1,2}\\b";
            for (t in ke) e += "|" + t + "\\b";
            return new RegExp(e + ")", "gi")
        }(), Ce = /hsl[a]?\(/, De = (St = Date.now, zt = 500, Rt = 33, Et = St(), Ft = Et, It = Bt = 1e3 / 240, Ct = {
            time: 0,
            frame: 0,
            tick: function () {
                Be(!0)
            },
            deltaRatio: function (t) {
                return Dt / (1e3 / (t || 60))
            },
            wake: function () {
                xt && (!vt && h() && (gt = vt = window, yt = gt.document || {}, ae.gsap = gr, (gt.gsapVersions || (gt.gsapVersions = [])).push(gr.version), l(Tt || gt.GreenSockGlobals || !gt.gsap && gt || {}), At = gt.requestAnimationFrame), Ot && Ct.sleep(), kt = At || function (t) {
                    return setTimeout(t, It - 1e3 * Ct.time + 1 | 0)
                }, Mt = 1, Be(2))
            },
            sleep: function () {
                (At ? gt.cancelAnimationFrame : clearTimeout)(Ot), Mt = 0, kt = d
            },
            lagSmoothing: function (t, e) {
                zt = t || 1e8, Rt = Math.min(e, zt, 0)
            },
            fps: function (t) {
                Bt = 1e3 / (t || 240), It = 1e3 * Ct.time + Bt
            },
            add: function (t, e, r) {
                var i = e ? function (e, r, n, s) {
                    t(e, r, n, s), Ct.remove(i)
                } : t;
                return Ct.remove(t), Lt[r ? "unshift" : "push"](i), Pe(), i
            },
            remove: function (t, e) {
                ~(e = Lt.indexOf(t)) && Lt.splice(e, 1) && e <= Pt && Pt--
            },
            _listeners: Lt = []
        }), Pe = function () {
            return !Mt && De.wake()
        }, Se = {}, ze = /^[\d.\-M][\d.\-,\s]/, Re = /["']/g, Ee = function (t) {
            return function (e) {
                return 1 - t(1 - e)
            }
        }, Fe = function (t, e) {
            return t && (n(t) ? t : Se[t] || function (t) {
                var e = (t + "").split("("), r = Se[e[0]];
                return r && 1 < e.length && r.config ? r.config.apply(null, ~t.indexOf("{") ? [function (t) {
                    for (var e, r, i, n = {}, s = t.substr(1, t.length - 3).split(":"), a = s[0], o = 1, u = s.length; o < u; o++) r = s[o], e = o !== u - 1 ? r.lastIndexOf(",") : r.length, i = r.substr(0, e), n[a] = isNaN(i) ? i.replace(Re, "").trim() : +i, a = r.substr(e + 1).trim();
                    return n
                }(e[1])] : function (t) {
                    var e = t.indexOf("(") + 1, r = t.indexOf(")"), i = t.indexOf("(", e);
                    return t.substring(e, ~i && i < r ? t.indexOf(")", r + 1) : r)
                }(t).split(",").map(k)) : Se._CE && ze.test(t) ? Se._CE("", t) : r
            }(t)) || e
        };

    function Be(t) {
        var e, r, i, n, s = St() - Ft, a = !0 === t;
        if (zt < s && (Et += s - Rt), (0 < (e = (i = (Ft += s) - Et) - It) || a) && (n = ++Ct.frame, Dt = i - 1e3 * Ct.time, Ct.time = i /= 1e3, It += e + (Bt <= e ? 4 : Bt - e), r = 1), a || (Ot = kt(Be)), r) for (Pt = 0; Pt < Lt.length; Pt++) Lt[Pt](i, Dt, n, t)
    }

    function Ie(t) {
        return t < Ut ? Yt * t * t : t < .7272727272727273 ? Yt * Math.pow(t - 1.5 / 2.75, 2) + .75 : t < .9090909090909092 ? Yt * (t -= 2.25 / 2.75) * t + .9375 : Yt * Math.pow(t - 2.625 / 2.75, 2) + .984375
    }

    y("Linear,Quad,Cubic,Quart,Quint,Strong", (function (t, e) {
        var r = e < 5 ? e + 1 : e;
        lt(t + ",Power" + (r - 1), e ? function (t) {
            return Math.pow(t, r)
        } : function (t) {
            return t
        }, (function (t) {
            return 1 - Math.pow(1 - t, r)
        }), (function (t) {
            return t < .5 ? Math.pow(2 * t, r) / 2 : 1 - Math.pow(2 * (1 - t), r) / 2
        }))
    })), Se.Linear.easeNone = Se.none = Se.Linear.easeIn, lt("Elastic", _t("in"), _t("out"), _t()), Yt = 7.5625, Ut = 1 / 2.75, lt("Bounce", (function (t) {
        return 1 - Ie(1 - t)
    }), Ie), lt("Expo", (function (t) {
        return t ? Math.pow(2, 10 * (t - 1)) : 0
    })), lt("Circ", (function (t) {
        return -(Wt(1 - t * t) - 1)
    })), lt("Sine", (function (t) {
        return 1 === t ? 1 : 1 - Ht(t * Qt)
    })), lt("Back", ct("in"), ct("out"), ct()), Se.SteppedEase = Se.steps = ae.SteppedEase = {
        config: function (t, e) {
            void 0 === t && (t = 1);
            var r = 1 / t, i = t + (e ? 0 : 1), n = e ? 1 : 0;
            return function (t) {
                return ((i * Te(0, .99999999, t) | 0) + n) * r
            }
        }
    }, Nt.ease = Se["quad.out"], y("onComplete,onUpdate,onStart,onRepeat,onReverseComplete,onInterrupt", (function (t) {
        return ce += t + "," + t + "Params,"
    }));
    var Le, Ye = function (t, e) {
        this.id = Gt++, (t._gsap = this).target = t, this.harness = e, this.get = e ? e.get : v, this.set = e ? e.getSetter : nr
    }, Ue = ((Le = Xe.prototype).delay = function (t) {
        return t || 0 === t ? (this.parent && this.parent.smoothChildTiming && this.startTime(this._start + t - this._delay), this._delay = t, this) : this._delay
    }, Le.duration = function (t) {
        return arguments.length ? this.totalDuration(0 < this._repeat ? t + (t + this._rDelay) * this._repeat : t) : this.totalDuration() && this._dur
    }, Le.totalDuration = function (t) {
        return arguments.length ? (this._dirty = 0, V(this, this._repeat < 0 ? t : (t - this._repeat * this._rDelay) / (this._repeat + 1))) : this._tDur
    }, Le.totalTime = function (t, e) {
        if (Pe(), !arguments.length) return this._tTime;
        var r = this._dp;
        if (r && r.smoothChildTiming && this._ts) {
            for (Y(this, t), !r._dp || r.parent || U(r, this); r && r.parent;) r.parent._time !== r._start + (0 <= r._ts ? r._tTime / r._ts : (r.totalDuration() - r._tTime) / -r._ts) && r.totalTime(r._tTime, !0), r = r.parent;
            !this.parent && this._dp.autoRemoveChildren && (0 < this._ts && t < this._tDur || this._ts < 0 && 0 < t || !this._tDur && !t) && X(this._dp, this, this._start - this._delay)
        }
        return (this._tTime !== t || !this._dur && !e || this._initted && Math.abs(this._zTime) === Vt || !t && !this._initted && (this.add || this._ptLookup)) && (this._ts || (this._pTime = t), O(this, t, e)), this
    }, Le.time = function (t, e) {
        return arguments.length ? this.totalTime(Math.min(this.totalDuration(), t + B(this)) % (this._dur + this._rDelay) || (t ? this._dur : 0), e) : this._time
    }, Le.totalProgress = function (t, e) {
        return arguments.length ? this.totalTime(this.totalDuration() * t, e) : this.totalDuration() ? Math.min(1, this._tTime / this._tDur) : this.ratio
    }, Le.progress = function (t, e) {
        return arguments.length ? this.totalTime(this.duration() * (!this._yoyo || 1 & this.iteration() ? t : 1 - t) + B(this), e) : this.duration() ? Math.min(1, this._time / this._dur) : this.ratio
    }, Le.iteration = function (t, e) {
        var r = this.duration() + this._rDelay;
        return arguments.length ? this.totalTime(this._time + (t - 1) * r, e) : this._repeat ? me(this._tTime, r) + 1 : 1
    }, Le.timeScale = function (t) {
        if (!arguments.length) return this._rts === -Vt ? 0 : this._rts;
        if (this._rts === t) return this;
        var e = this.parent && this._ts ? I(this.parent._time, this) : this._tTime;
        return this._rts = +t || 0, this._ts = this._ps || t === -Vt ? 0 : this._rts, this.totalTime(Te(-this._delay, this._tDur, e), !0), L(this), function (t) {
            for (var e = t.parent; e && e.parent;) e._dirty = 1, e.totalDuration(), e = e.parent;
            return t
        }(this)
    }, Le.paused = function (t) {
        return arguments.length ? (this._ps !== t && ((this._ps = t) ? (this._pTime = this._tTime || Math.max(-this._delay, this.rawTime()), this._ts = this._act = 0) : (Pe(), this._ts = this._rts, this.totalTime(this.parent && !this.parent.smoothChildTiming ? this.rawTime() : this._tTime || this._pTime, 1 === this.progress() && Math.abs(this._zTime) !== Vt && (this._tTime -= Vt)))), this) : this._ps
    }, Le.startTime = function (t) {
        if (arguments.length) {
            this._start = t;
            var e = this.parent || this._dp;
            return !e || !e._sort && this.parent || X(e, this, t - this._delay), this
        }
        return this._start
    }, Le.endTime = function (t) {
        return this._start + (u(t) ? this.totalDuration() : this.duration()) / Math.abs(this._ts || 1)
    }, Le.rawTime = function (t) {
        var e = this.parent || this._dp;
        return e ? t && (!this._ts || this._repeat && this._time && this.totalProgress() < 1) ? this._tTime % (this._dur + this._rDelay) : this._ts ? I(e.rawTime(t), this) : this._tTime : this._tTime
    }, Le.globalTime = function (t) {
        for (var e = this, r = arguments.length ? t : e.rawTime(); e;) r = e._start + r / (e._ts || 1), e = e._dp;
        return r
    }, Le.repeat = function (t) {
        return arguments.length ? (this._repeat = t === 1 / 0 ? -2 : t, j(this)) : -2 === this._repeat ? 1 / 0 : this._repeat
    }, Le.repeatDelay = function (t) {
        if (arguments.length) {
            var e = this._time;
            return this._rDelay = t, j(this), e ? this.time(e) : this
        }
        return this._rDelay
    }, Le.yoyo = function (t) {
        return arguments.length ? (this._yoyo = t, this) : this._yoyo
    }, Le.seek = function (t, e) {
        return this.totalTime(ye(this, t), u(e))
    }, Le.restart = function (t, e) {
        return this.play().totalTime(t ? -this._delay : 0, u(e))
    }, Le.play = function (t, e) {
        return null != t && this.seek(t, e), this.reversed(!1).paused(!1)
    }, Le.reverse = function (t, e) {
        return null != t && this.seek(t || this.totalDuration(), e), this.reversed(!0).paused(!1)
    }, Le.pause = function (t, e) {
        return null != t && this.seek(t, e), this.paused(!0)
    }, Le.resume = function () {
        return this.paused(!1)
    }, Le.reversed = function (t) {
        return arguments.length ? (!!t !== this.reversed() && this.timeScale(-this._rts || (t ? -Vt : 0)), this) : this._rts < 0
    }, Le.invalidate = function () {
        return this._initted = this._act = 0, this._zTime = -Vt, this
    }, Le.isActive = function () {
        var t, e = this.parent || this._dp, r = this._start;
        return !(e && !(this._ts && this._initted && e.isActive() && (t = e.rawTime(!0)) >= r && t < this.endTime(!0) - Vt))
    }, Le.eventCallback = function (t, e, r) {
        var i = this.vars;
        return 1 < arguments.length ? (e ? (i[t] = e, r && (i[t + "Params"] = r), "onUpdate" === t && (this._onUpdate = e)) : delete i[t], this) : i[t]
    }, Le.then = function (t) {
        var e = this;
        return new Promise((function (r) {
            function i() {
                var t = e.then;
                e.then = null, n(s) && (s = s(e)) && (s.then || s === e) && (e.then = t), r(s), e.then = t
            }

            var s = n(t) ? t : A;
            e._initted && 1 === e.totalProgress() && 0 <= e._ts || !e._tTime && e._ts < 0 ? i() : e._prom = i
        }))
    }, Le.kill = function () {
        nt(this)
    }, Xe);

    function Xe(t) {
        this.vars = t, this._delay = +t.delay || 0, (this._repeat = t.repeat === 1 / 0 ? -2 : t.repeat || 0) && (this._rDelay = t.repeatDelay || 0, this._yoyo = !!t.yoyo || !!t.yoyoEase), this._ts = 1, V(this, +t.duration, 1, 1), this.data = t.data, Mt || De.wake()
    }

    C(Ue.prototype, {
        _time: 0,
        _start: 0,
        _end: 0,
        _tTime: 0,
        _tDur: 0,
        _dirty: 0,
        _repeat: 0,
        _yoyo: !1,
        parent: null,
        _initted: !1,
        _rDelay: 0,
        _ts: 1,
        _dp: 0,
        ratio: 0,
        _zTime: -Vt,
        _prom: 0,
        _ps: !1,
        _rts: 1
    });
    var Ne = function (t) {
        function a(e, i) {
            var n;
            return void 0 === e && (e = {}), (n = t.call(this, e) || this).labels = {}, n.smoothChildTiming = !!e.smoothChildTiming, n.autoRemoveChildren = !!e.autoRemoveChildren, n._sort = u(e.sortChildren), mt && X(e.parent || mt, r(n), i), e.reversed && n.reverse(), e.paused && n.paused(!0), e.scrollTrigger && N(r(n), e.scrollTrigger), n
        }

        e(a, t);
        var o = a.prototype;
        return o.to = function (t, e, r) {
            return Q(0, arguments, this), this
        }, o.from = function (t, e, r) {
            return Q(1, arguments, this), this
        }, o.fromTo = function (t, e, r, i) {
            return Q(2, arguments, this), this
        }, o.set = function (t, e, r) {
            return e.duration = 0, e.parent = this, S(e).repeatDelay || (e.repeat = 0), e.immediateRender = !!e.immediateRender, new Je(t, e, ye(this, r), 1), this
        }, o.call = function (t, e, r) {
            return X(this, Je.delayedCall(0, t, e), r)
        }, o.staggerTo = function (t, e, r, i, n, s, a) {
            return r.duration = e, r.stagger = r.stagger || i, r.onComplete = s, r.onCompleteParams = a, r.parent = this, new Je(t, r, ye(this, n)), this
        }, o.staggerFrom = function (t, e, r, i, n, s, a) {
            return r.runBackwards = 1, S(r).immediateRender = u(r.immediateRender), this.staggerTo(t, e, r, i, n, s, a)
        }, o.staggerFromTo = function (t, e, r, i, n, s, a, o) {
            return i.startAt = r, S(i).immediateRender = u(i.immediateRender), this.staggerTo(t, e, i, n, s, a, o)
        }, o.render = function (t, e, r) {
            var i, n, s, a, o, u, h, f, l, p, _, c, d = this._time, m = this._dirty ? this.totalDuration() : this._tDur,
                g = this._dur, v = t <= 0 ? 0 : x(t), y = this._zTime < 0 != t < 0 && (this._initted || !g);
            if (this !== mt && m < v && 0 <= t && (v = m), v !== this._tTime || r || y) {
                if (d !== this._time && g && (v += this._time - d, t += this._time - d), i = v, l = this._start, u = !(f = this._ts), y && (g || (d = this._zTime), !t && e || (this._zTime = t)), this._repeat) {
                    if (_ = this._yoyo, o = g + this._rDelay, this._repeat < -1 && t < 0) return this.totalTime(100 * o + t, e, r);
                    if (i = x(v % o), v === m ? (a = this._repeat, i = g) : ((a = ~~(v / o)) && a === v / o && (i = g, a--), g < i && (i = g)), p = me(this._tTime, o), !d && this._tTime && p !== a && (p = a), _ && 1 & a && (i = g - i, c = 1), a !== p && !this._lock) {
                        var T = _ && 1 & p, w = T === (_ && 1 & a);
                        if (a < p && (T = !T), d = T ? 0 : g, this._lock = 1, this.render(d || (c ? 0 : x(a * o)), e, !g)._lock = 0, this._tTime = v, !e && this.parent && Me(this, "onRepeat"), this.vars.repeatRefresh && !c && (this.invalidate()._lock = 1), d && d !== this._time || u != !this._ts || this.vars.onRepeat && !this.parent && !this._act) return this;
                        if (g = this._dur, m = this._tDur, w && (this._lock = 2, d = T ? g : -1e-4, this.render(d, !0), this.vars.repeatRefresh && !c && this.invalidate()), this._lock = 0, !this._ts && !u) return this;
                        ft(this, c)
                    }
                }
                if (this._hasPause && !this._forcing && this._lock < 2 && (h = function (t, e, r) {
                    var i;
                    if (e < r) for (i = t._first; i && i._start <= r;) {
                        if ("isPause" === i.data && i._start > e) return i;
                        i = i._next
                    } else for (i = t._last; i && i._start >= r;) {
                        if ("isPause" === i.data && i._start < e) return i;
                        i = i._prev
                    }
                }(this, x(d), x(i))) && (v -= i - (i = h._start)), this._tTime = v, this._time = i, this._act = !f, this._initted || (this._onUpdate = this.vars.onUpdate, this._initted = 1, this._zTime = t, d = 0), !d && i && !e && (Me(this, "onStart"), this._tTime !== v)) return this;
                if (d <= i && 0 <= t) for (n = this._first; n;) {
                    if (s = n._next, (n._act || i >= n._start) && n._ts && h !== n) {
                        if (n.parent !== this) return this.render(t, e, r);
                        if (n.render(0 < n._ts ? (i - n._start) * n._ts : (n._dirty ? n.totalDuration() : n._tDur) + (i - n._start) * n._ts, e, r), i !== this._time || !this._ts && !u) {
                            h = 0, s && (v += this._zTime = -Vt);
                            break
                        }
                    }
                    n = s
                } else {
                    n = this._last;
                    for (var b = t < 0 ? t : i; n;) {
                        if (s = n._prev, (n._act || b <= n._end) && n._ts && h !== n) {
                            if (n.parent !== this) return this.render(t, e, r);
                            if (n.render(0 < n._ts ? (b - n._start) * n._ts : (n._dirty ? n.totalDuration() : n._tDur) + (b - n._start) * n._ts, e, r), i !== this._time || !this._ts && !u) {
                                h = 0, s && (v += this._zTime = b ? -Vt : Vt);
                                break
                            }
                        }
                        n = s
                    }
                }
                if (h && !e && (this.pause(), h.render(d <= i ? 0 : -Vt)._zTime = d <= i ? 1 : -1, this._ts)) return this._start = l, L(this), this.render(t, e, r);
                this._onUpdate && !e && Me(this, "onUpdate", !0), (v === m && this._tTime >= this.totalDuration() || !v && d) && (l !== this._start && Math.abs(f) === Math.abs(this._ts) || this._lock || (!t && g || !(v === m && 0 < this._ts || !v && this._ts < 0) || E(this, 1), e || t < 0 && !d || !v && !d && m || (Me(this, v === m && 0 <= t ? "onComplete" : "onReverseComplete", !0), !this._prom || v < m && 0 < this.timeScale() || this._prom())))
            }
            return this
        }, o.add = function (t, e) {
            var r = this;
            if (s(e) || (e = ye(this, e, t)), !(t instanceof Ue)) {
                if (Jt(t)) return t.forEach((function (t) {
                    return r.add(t, e)
                })), this;
                if (i(t)) return this.addLabel(t, e);
                if (!n(t)) return this;
                t = Je.delayedCall(0, t)
            }
            return this !== t ? X(this, t, e) : this
        }, o.getChildren = function (t, e, r, i) {
            void 0 === t && (t = !0), void 0 === e && (e = !0), void 0 === r && (r = !0), void 0 === i && (i = -qt);
            for (var n = [], s = this._first; s;) s._start >= i && (s instanceof Je ? e && n.push(s) : (r && n.push(s), t && n.push.apply(n, s.getChildren(!0, e, r)))), s = s._next;
            return n
        }, o.getById = function (t) {
            for (var e = this.getChildren(1, 1, 1), r = e.length; r--;) if (e[r].vars.id === t) return e[r]
        }, o.remove = function (t) {
            return i(t) ? this.removeLabel(t) : n(t) ? this.killTweensOf(t) : (R(this, t), t === this._recent && (this._recent = this._last), F(this))
        }, o.totalTime = function (e, r) {
            return arguments.length ? (this._forcing = 1, !this._dp && this._ts && (this._start = x(De.time - (0 < this._ts ? e / this._ts : (this.totalDuration() - e) / -this._ts))), t.prototype.totalTime.call(this, e, r), this._forcing = 0, this) : this._tTime
        }, o.addLabel = function (t, e) {
            return this.labels[t] = ye(this, e), this
        }, o.removeLabel = function (t) {
            return delete this.labels[t], this
        }, o.addPause = function (t, e, r) {
            var i = Je.delayedCall(0, e || d, r);
            return i.data = "isPause", this._hasPause = 1, X(this, i, ye(this, t))
        }, o.removePause = function (t) {
            var e = this._first;
            for (t = ye(this, t); e;) e._start === t && "isPause" === e.data && E(e), e = e._next
        }, o.killTweensOf = function (t, e, r) {
            for (var i = this.getTweensOf(t, r), n = i.length; n--;) je !== i[n] && i[n].kill(t, e);
            return this
        }, o.getTweensOf = function (t, e) {
            for (var r, i = [], n = we(t), a = this._first, o = s(e); a;) a instanceof Je ? b(a._targets, n) && (o ? (!je || a._initted && a._ts) && a.globalTime(0) <= e && a.globalTime(a.totalDuration()) > e : !e || a.isActive()) && i.push(a) : (r = a.getTweensOf(n, e)).length && i.push.apply(i, r), a = a._next;
            return i
        }, o.tweenTo = function (t, e) {
            e = e || {};
            var r, i = this, n = ye(i, t), s = e.startAt, a = e.onStart, o = e.onStartParams, u = e.immediateRender,
                h = Je.to(i, C({
                    ease: e.ease || "none",
                    lazy: !1,
                    immediateRender: !1,
                    time: n,
                    overwrite: "auto",
                    duration: e.duration || Math.abs((n - (s && "time" in s ? s.time : i._time)) / i.timeScale()) || Vt,
                    onStart: function () {
                        if (i.pause(), !r) {
                            var t = e.duration || Math.abs((n - (s && "time" in s ? s.time : i._time)) / i.timeScale());
                            h._dur !== t && V(h, t, 0, 1).render(h._time, !0, !0), r = 1
                        }
                        a && a.apply(h, o || [])
                    }
                }, e));
            return u ? h.render(0) : h
        }, o.tweenFromTo = function (t, e, r) {
            return this.tweenTo(e, C({startAt: {time: ye(this, t)}}, r))
        }, o.recent = function () {
            return this._recent
        }, o.nextLabel = function (t) {
            return void 0 === t && (t = this._time), it(this, ye(this, t))
        }, o.previousLabel = function (t) {
            return void 0 === t && (t = this._time), it(this, ye(this, t), 1)
        }, o.currentLabel = function (t) {
            return arguments.length ? this.seek(t, !0) : this.previousLabel(this._time + Vt)
        }, o.shiftChildren = function (t, e, r) {
            void 0 === r && (r = 0);
            for (var i, n = this._first, s = this.labels; n;) n._start >= r && (n._start += t, n._end += t), n = n._next;
            if (e) for (i in s) s[i] >= r && (s[i] += t);
            return F(this)
        }, o.invalidate = function () {
            var e = this._first;
            for (this._lock = 0; e;) e.invalidate(), e = e._next;
            return t.prototype.invalidate.call(this)
        }, o.clear = function (t) {
            void 0 === t && (t = !0);
            for (var e, r = this._first; r;) e = r._next, this.remove(r), r = e;
            return this._dp && (this._time = this._tTime = this._pTime = 0), t && (this.labels = {}), F(this)
        }, o.totalDuration = function (t) {
            var e, r, i, n = 0, s = this, a = s._last, o = qt;
            if (arguments.length) return s.timeScale((s._repeat < 0 ? s.duration() : s.totalDuration()) / (s.reversed() ? -t : t));
            if (s._dirty) {
                for (i = s.parent; a;) e = a._prev, a._dirty && a.totalDuration(), o < (r = a._start) && s._sort && a._ts && !s._lock ? (s._lock = 1, X(s, a, r - a._delay, 1)._lock = 0) : o = r, r < 0 && a._ts && (n -= r, (!i && !s._dp || i && i.smoothChildTiming) && (s._start += r / s._ts, s._time -= r, s._tTime -= r), s.shiftChildren(-r, !1, -1 / 0), o = 0), a._end > n && a._ts && (n = a._end), a = e;
                V(s, s === mt && s._time > n ? s._time : n, 1, 1), s._dirty = 0
            }
            return s._tDur
        }, a.updateRoot = function (t) {
            if (mt._ts && (O(mt, I(t, mt)), wt = De.frame), De.frame >= pe) {
                pe += Xt.autoSleep || 120;
                var e = mt._first;
                if ((!e || !e._ts) && Xt.autoSleep && De._listeners.length < 2) {
                    for (; e && !e._ts;) e = e._next;
                    e || De.sleep()
                }
            }
        }, a
    }(Ue);

    function qe(t, e, r, s, a, u) {
        var h, f, l, p;
        if (fe[t] && !1 !== (h = new fe[t]).init(a, h.rawVars ? e[t] : function (t, e, r, s, a) {
            if (n(t) && (t = He(t, a, e, r, s)), !o(t) || t.style && t.nodeType || Jt(t) || $t(t)) return i(t) ? He(t, a, e, r, s) : t;
            var u, h = {};
            for (u in t) h[u] = He(t[u], a, e, r, s);
            return h
        }(e[t], s, a, u, r), r, s, u) && (r._pt = f = new pr(r._pt, a, t, 0, 1, h.render, h, 0, h.priority), r !== bt)) for (l = r._ptLookup[r._targets.indexOf(a)], p = h._props.length; p--;) l[h._props[p]] = f;
        return h
    }

    function Ve(t, e, r, i) {
        var n, s, a = e.ease || i || "power1.inOut";
        if (Jt(e)) s = r[t] || (r[t] = []), e.forEach((function (t, r) {
            return s.push({t: r / (e.length - 1) * 100, v: t, e: a})
        })); else for (n in e) s = r[n] || (r[n] = []), "ease" === n || s.push({t: parseFloat(t), v: e[n], e: a})
    }

    C(Ne.prototype, {_lock: 0, _hasPause: 0, _forcing: 0});
    var je, Qe, Ge = function (t, e, r, s, a, o, u, h, f) {
        n(s) && (s = s(a || 0, t, o));
        var l, _ = t[e],
            c = "get" !== r ? r : n(_) ? f ? t[e.indexOf("set") || !n(t["get" + e.substr(3)]) ? e : "get" + e.substr(3)](f) : t[e]() : _,
            d = n(_) ? f ? ir : rr : er;
        if (i(s) && (~s.indexOf("random(") && (s = rt(s)), "=" === s.charAt(1) && (!(l = w(c, s) + (W(c) || 0)) && 0 !== l || (s = l))), c !== s || Qe) return isNaN(c * s) || "" === s ? (_ || e in t || p(e, s), function (t, e, r, i, n, s, a) {
            var o, u, h, f, l, p, _, c, d = new pr(this._pt, t, e, 0, 1, or, null, n), m = 0, g = 0;
            for (d.b = r, d.e = i, r += "", (_ = ~(i += "").indexOf("random(")) && (i = rt(i)), s && (s(c = [r, i], t, e), r = c[0], i = c[1]), u = r.match(re) || []; o = re.exec(i);) f = o[0], l = i.substring(m, o.index), h ? h = (h + 1) % 5 : "rgba(" === l.substr(-5) && (h = 1), f !== u[g++] && (p = parseFloat(u[g - 1]) || 0, d._pt = {
                _next: d._pt,
                p: l || 1 === g ? l : ",",
                s: p,
                c: "=" === f.charAt(1) ? w(p, f) - p : parseFloat(f) - p,
                m: h && h < 4 ? Math.round : 0
            }, m = re.lastIndex);
            return d.c = m < i.length ? i.substring(m, i.length) : "", d.fp = a, (ie.test(i) || _) && (d.e = 0), this._pt = d
        }.call(this, t, e, c, s, d, h || Xt.stringFilter, f)) : (l = new pr(this._pt, t, e, +c || 0, s - (c || 0), "boolean" == typeof _ ? ar : sr, 0, d), f && (l.fp = f), u && l.modifier(u, this, t), this._pt = l)
    }, We = function t(e, r) {
        var i, n, s, a, o, h, f, l, p, _, c, d, v, y = e.vars, T = y.ease, x = y.startAt, w = y.immediateRender,
            b = y.lazy, O = y.onUpdate, k = y.onUpdateParams, A = y.callbackScope, D = y.runBackwards, S = y.yoyoEase,
            z = y.keyframes, R = y.autoRevert, F = e._dur, B = e._startAt, I = e._targets, L = e.parent,
            Y = L && "nested" === L.data ? L.parent._targets : I, U = "auto" === e._overwrite && !dt, X = e.timeline;
        if (!X || z && T || (T = "none"), e._ease = Fe(T, Nt.ease), e._yEase = S ? Ee(Fe(!0 === S ? T : S, Nt.ease)) : 0, S && e._yoyo && !e._repeat && (S = e._yEase, e._yEase = e._ease, e._ease = S), e._from = !X && !!y.runBackwards, !X || z && !y.stagger) {
            if (d = (l = I[0] ? g(I[0]).harness : 0) && y[l.prop], i = P(y, oe), B && (E(B.render(-1, !0)), B._lazy = 0), x) if (E(e._startAt = Je.set(I, C({
                data: "isStart",
                overwrite: !1,
                parent: L,
                immediateRender: !0,
                lazy: u(b),
                startAt: null,
                delay: 0,
                onUpdate: O,
                onUpdateParams: k,
                callbackScope: A,
                stagger: 0
            }, x))), r < 0 && !w && !R && e._startAt.render(-1, !0), w) {
                if (0 < r && !R && (e._startAt = 0), F && r <= 0) return void (r && (e._zTime = r))
            } else !1 === R && (e._startAt = 0); else if (D && F) if (B) R || (e._startAt = 0); else if (r && (w = !1), s = C({
                overwrite: !1,
                data: "isFromStart",
                lazy: w && u(b),
                immediateRender: w,
                stagger: 0,
                parent: L
            }, i), d && (s[l.prop] = d), E(e._startAt = Je.set(I, s)), r < 0 && e._startAt.render(-1, !0), e._zTime = r, w) {
                if (!r) return
            } else t(e._startAt, Vt);
            for (e._pt = e._ptCache = 0, b = F && u(b) || b && !F, n = 0; n < I.length; n++) {
                if (f = (o = I[n])._gsap || m(I)[n]._gsap, e._ptLookup[n] = _ = {}, he[f.id] && ue.length && M(), c = Y === I ? n : Y.indexOf(o), l && !1 !== (p = new l).init(o, d || i, e, c, Y) && (e._pt = a = new pr(e._pt, o, p.name, 0, 1, p.render, p, 0, p.priority), p._props.forEach((function (t) {
                    _[t] = a
                })), p.priority && (h = 1)), !l || d) for (s in i) fe[s] && (p = qe(s, i, e, c, o, Y)) ? p.priority && (h = 1) : _[s] = a = Ge.call(e, o, s, "get", i[s], c, Y, 0, y.stringFilter);
                e._op && e._op[n] && e.kill(o, e._op[n]), U && e._pt && (je = e, mt.killTweensOf(o, _, e.globalTime(r)), v = !e.parent, je = 0), e._pt && b && (he[f.id] = 1)
            }
            h && lr(e), e._onInit && e._onInit(e)
        }
        e._onUpdate = O, e._initted = (!e._op || e._pt) && !v, z && r <= 0 && X.render(qt, !0, !0)
    }, He = function (t, e, r, s, a) {
        return n(t) ? t.call(e, r, s, a) : i(t) && ~t.indexOf("random(") ? rt(t) : t
    }, Ze = ce + "repeat,repeatDelay,yoyo,repeatRefresh,yoyoEase,autoRevert", $e = {};
    y(Ze + ",id,stagger,delay,duration,paused,scrollTrigger", (function (t) {
        return $e[t] = 1
    }));
    var Je = function (t) {
        function n(e, i, n, a) {
            var h;
            "number" == typeof i && (n.duration = i, i = n, n = null);
            var l, p, c, d, g, v, y, T, w = (h = t.call(this, a ? i : S(i)) || this).vars, b = w.duration, M = w.delay,
                O = w.immediateRender, k = w.stagger, A = w.overwrite, D = w.keyframes, z = w.defaults,
                R = w.scrollTrigger, E = w.yoyoEase, F = i.parent || mt,
                B = (Jt(e) || $t(e) ? s(e[0]) : "length" in i) ? [e] : we(e);
            if (h._targets = B.length ? m(B) : _("GSAP target " + e + " not found. https://greensock.com", !Xt.nullTargetWarn) || [], h._ptLookup = [], h._overwrite = A, D || k || f(b) || f(M)) {
                if (i = h.vars, (l = h.timeline = new Ne({
                    data: "nested",
                    defaults: z || {}
                })).kill(), l.parent = l._dp = r(h), l._start = 0, k || f(b) || f(M)) {
                    if (d = B.length, y = k && $(k), o(k)) for (g in k) ~Ze.indexOf(g) && ((T = T || {})[g] = k[g]);
                    for (p = 0; p < d; p++) (c = P(i, $e)).stagger = 0, E && (c.yoyoEase = E), T && de(c, T), v = B[p], c.duration = +He(b, r(h), p, v, B), c.delay = (+He(M, r(h), p, v, B) || 0) - h._delay, !k && 1 === d && c.delay && (h._delay = M = c.delay, h._start += M, c.delay = 0), l.to(v, c, y ? y(p, v, B) : 0), l._ease = Se.none;
                    l.duration() ? b = M = 0 : h.timeline = 0
                } else if (D) {
                    S(C(l.vars.defaults, {ease: "none"})), l._ease = Fe(D.ease || i.ease || "none");
                    var I, L, Y, U = 0;
                    if (Jt(D)) D.forEach((function (t) {
                        return l.to(B, t, ">")
                    })); else {
                        for (g in c = {}, D) "ease" === g || "easeEach" === g || Ve(g, D[g], c, D.easeEach);
                        for (g in c) for (I = c[g].sort((function (t, e) {
                            return t.t - e.t
                        })), p = U = 0; p < I.length; p++) (Y = {
                            ease: (L = I[p]).e,
                            duration: (L.t - (p ? I[p - 1].t : 0)) / 100 * b
                        })[g] = L.v, l.to(B, Y, U), U += Y.duration;
                        l.duration() < b && l.to({}, {duration: b - l.duration()})
                    }
                }
                b || h.duration(b = l.duration())
            } else h.timeline = 0;
            return !0 !== A || dt || (je = r(h), mt.killTweensOf(B), je = 0), X(F, r(h), n), i.reversed && h.reverse(), i.paused && h.paused(!0), (O || !b && !D && h._start === x(F._time) && u(O) && function t(e) {
                return !e || e._ts && t(e.parent)
            }(r(h)) && "nested" !== F.data) && (h._tTime = -Vt, h.render(Math.max(0, -M))), R && N(r(h), R), h
        }

        e(n, t);
        var a = n.prototype;
        return a.render = function (t, e, r) {
            var i, n, s, a, o, u, h, f, l, p = this._time, _ = this._tDur, c = this._dur,
                d = _ - Vt < t && 0 <= t ? _ : t < Vt ? 0 : t;
            if (c) {
                if (d !== this._tTime || !t || r || !this._initted && this._tTime || this._startAt && this._zTime < 0 != t < 0) {
                    if (i = d, f = this.timeline, this._repeat) {
                        if (a = c + this._rDelay, this._repeat < -1 && t < 0) return this.totalTime(100 * a + t, e, r);
                        if (i = x(d % a), d === _ ? (s = this._repeat, i = c) : ((s = ~~(d / a)) && s === d / a && (i = c, s--), c < i && (i = c)), (u = this._yoyo && 1 & s) && (l = this._yEase, i = c - i), o = me(this._tTime, a), i === p && !r && this._initted) return this._tTime = d, this;
                        s !== o && (f && this._yEase && ft(f, u), !this.vars.repeatRefresh || u || this._lock || (this._lock = r = 1, this.render(x(a * s), !0).invalidate()._lock = 0))
                    }
                    if (!this._initted) {
                        if (q(this, t < 0 ? t : i, r, e)) return this._tTime = 0, this;
                        if (p !== this._time) return this;
                        if (c !== this._dur) return this.render(t, e, r)
                    }
                    if (this._tTime = d, this._time = i, !this._act && this._ts && (this._act = 1, this._lazy = 0), this.ratio = h = (l || this._ease)(i / c), this._from && (this.ratio = h = 1 - h), i && !p && !e && (Me(this, "onStart"), this._tTime !== d)) return this;
                    for (n = this._pt; n;) n.r(h, n.d), n = n._next;
                    f && f.render(t < 0 ? t : !i && u ? -Vt : f._dur * f._ease(i / this._dur), e, r) || this._startAt && (this._zTime = t), this._onUpdate && !e && (t < 0 && this._startAt && this._startAt.render(t, !0, r), Me(this, "onUpdate")), this._repeat && s !== o && this.vars.onRepeat && !e && this.parent && Me(this, "onRepeat"), d !== this._tDur && d || this._tTime !== d || (t < 0 && this._startAt && !this._onUpdate && this._startAt.render(t, !0, !0), !t && c || !(d === this._tDur && 0 < this._ts || !d && this._ts < 0) || E(this, 1), e || t < 0 && !p || !d && !p || (Me(this, d === _ ? "onComplete" : "onReverseComplete", !0), !this._prom || d < _ && 0 < this.timeScale() || this._prom()))
                }
            } else !function (t, e, r, i) {
                var n, s, a, o = t.ratio, u = e < 0 || !e && (!t._start && function t(e) {
                    var r = e.parent;
                    return r && r._ts && r._initted && !r._lock && (r.rawTime() < 0 || t(r))
                }(t) && (t._initted || !ge(t)) || (t._ts < 0 || t._dp._ts < 0) && !ge(t)) ? 0 : 1, h = t._rDelay, f = 0;
                if (h && t._repeat && (f = Te(0, t._tDur, e), s = me(f, h), t._yoyo && 1 & s && (u = 1 - u), s !== me(t._tTime, h) && (o = 1 - u, t.vars.repeatRefresh && t._initted && t.invalidate())), u !== o || i || t._zTime === Vt || !e && t._zTime) {
                    if (!t._initted && q(t, e, i, r)) return;
                    for (a = t._zTime, t._zTime = e || (r ? Vt : 0), r = r || e && !a, t.ratio = u, t._from && (u = 1 - u), t._time = 0, t._tTime = f, n = t._pt; n;) n.r(u, n.d), n = n._next;
                    t._startAt && e < 0 && t._startAt.render(e, !0, !0), t._onUpdate && !r && Me(t, "onUpdate"), f && t._repeat && !r && t.parent && Me(t, "onRepeat"), (e >= t._tDur || e < 0) && t.ratio === u && (u && E(t, 1), r || (Me(t, u ? "onComplete" : "onReverseComplete", !0), t._prom && t._prom()))
                } else t._zTime || (t._zTime = e)
            }(this, t, e, r);
            return this
        }, a.targets = function () {
            return this._targets
        }, a.invalidate = function () {
            return this._pt = this._op = this._startAt = this._onUpdate = this._lazy = this.ratio = 0, this._ptLookup = [], this.timeline && this.timeline.invalidate(), t.prototype.invalidate.call(this)
        }, a.resetTo = function (t, e, r, i) {
            Mt || De.wake(), this._ts || this.play();
            var n = Math.min(this._dur, (this._dp._time - this._start) * this._ts);
            return this._initted || We(this, n), function (t, e, r, i, n, s, a) {
                var o, u, h, f = (t._pt && t._ptCache || (t._ptCache = {}))[e];
                if (!f) for (f = t._ptCache[e] = [], u = t._ptLookup, h = t._targets.length; h--;) {
                    if ((o = u[h][e]) && o.d && o.d._pt) for (o = o.d._pt; o && o.p !== e;) o = o._next;
                    if (!o) return Qe = 1, t.vars[e] = "+=0", We(t, a), Qe = 0, 1;
                    f.push(o)
                }
                for (h = f.length; h--;) (o = f[h]).s = !i && 0 !== i || n ? o.s + (i || 0) + s * o.c : i, o.c = r - o.s, o.e && (o.e = T(r) + W(o.e)), o.b && (o.b = o.s + W(o.b))
            }(this, t, e, r, i, this._ease(n / this._dur), n) ? this.resetTo(t, e, r, i) : (Y(this, 0), this.parent || z(this._dp, this, "_first", "_last", this._dp._sort ? "_start" : 0), this.render(0))
        }, a.kill = function (t, e) {
            if (void 0 === e && (e = "all"), !(t || e && "all" !== e)) return this._lazy = this._pt = 0, this.parent ? nt(this) : this;
            if (this.timeline) {
                var r = this.timeline.totalDuration();
                return this.timeline.killTweensOf(t, e, je && !0 !== je.vars.overwrite)._first || nt(this), this.parent && r !== this.timeline.totalDuration() && V(this, this._dur * this.timeline._tDur / r, 0, 1), this
            }
            var n, s, a, o, u, h, f, l = this._targets, p = t ? we(t) : l, _ = this._ptLookup, c = this._pt;
            if ((!e || "all" === e) && function (t, e) {
                for (var r = t.length, i = r === e.length; i && r-- && t[r] === e[r];) ;
                return r < 0
            }(l, p)) return "all" === e && (this._pt = 0), nt(this);
            for (n = this._op = this._op || [], "all" !== e && (i(e) && (u = {}, y(e, (function (t) {
                return u[t] = 1
            })), e = u), e = function (t, e) {
                var r, i, n, s, a = t[0] ? g(t[0]).harness : 0, o = a && a.aliases;
                if (!o) return e;
                for (i in r = de({}, e), o) if (i in r) for (n = (s = o[i].split(",")).length; n--;) r[s[n]] = r[i];
                return r
            }(l, e)), f = l.length; f--;) if (~p.indexOf(l[f])) for (u in s = _[f], "all" === e ? (n[f] = e, o = s, a = {}) : (a = n[f] = n[f] || {}, o = e), o) (h = s && s[u]) && ("kill" in h.d && !0 !== h.d.kill(u) || R(this, h, "_pt"), delete s[u]), "all" !== a && (a[u] = 1);
            return this._initted && !this._pt && c && nt(this), this
        }, n.to = function (t, e, r) {
            return new n(t, e, r)
        }, n.from = function (t, e) {
            return Q(1, arguments)
        }, n.delayedCall = function (t, e, r, i) {
            return new n(e, 0, {
                immediateRender: !1,
                lazy: !1,
                overwrite: !1,
                delay: t,
                onComplete: e,
                onReverseComplete: e,
                onCompleteParams: r,
                onReverseCompleteParams: r,
                callbackScope: i
            })
        }, n.fromTo = function (t, e, r) {
            return Q(2, arguments)
        }, n.set = function (t, e) {
            return e.duration = 0, e.repeatDelay || (e.repeat = 0), new n(t, e)
        }, n.killTweensOf = function (t, e, r) {
            return mt.killTweensOf(t, e, r)
        }, n
    }(Ue);

    function Ke(t, e, r) {
        return t.setAttribute(e, r)
    }

    function tr(t, e, r, i) {
        i.mSet(t, e, i.m.call(i.tween, r, i.mt), i)
    }

    C(Je.prototype, {
        _targets: [],
        _lazy: 0,
        _startAt: 0,
        _op: 0,
        _onInit: 0
    }), y("staggerTo,staggerFrom,staggerFromTo", (function (t) {
        Je[t] = function () {
            var e = new Ne, r = xe.call(arguments, 0);
            return r.splice("staggerFromTo" === t ? 5 : 4, 0, 0), e[t].apply(e, r)
        }
    }));
    var er = function (t, e, r) {
        return t[e] = r
    }, rr = function (t, e, r) {
        return t[e](r)
    }, ir = function (t, e, r, i) {
        return t[e](i.fp, r)
    }, nr = function (t, e) {
        return n(t[e]) ? rr : a(t[e]) && t.setAttribute ? Ke : er
    }, sr = function (t, e) {
        return e.set(e.t, e.p, Math.round(1e6 * (e.s + e.c * t)) / 1e6, e)
    }, ar = function (t, e) {
        return e.set(e.t, e.p, !!(e.s + e.c * t), e)
    }, or = function (t, e) {
        var r = e._pt, i = "";
        if (!t && e.b) i = e.b; else if (1 === t && e.e) i = e.e; else {
            for (; r;) i = r.p + (r.m ? r.m(r.s + r.c * t) : Math.round(1e4 * (r.s + r.c * t)) / 1e4) + i, r = r._next;
            i += e.c
        }
        e.set(e.t, e.p, i, e)
    }, ur = function (t, e) {
        for (var r = e._pt; r;) r.r(t, r.d), r = r._next
    }, hr = function (t, e, r, i) {
        for (var n, s = this._pt; s;) n = s._next, s.p === i && s.modifier(t, e, r), s = n
    }, fr = function (t) {
        for (var e, r, i = this._pt; i;) r = i._next, i.p === t && !i.op || i.op === t ? R(this, i, "_pt") : i.dep || (e = 1), i = r;
        return !e
    }, lr = function (t) {
        for (var e, r, i, n, s = t._pt; s;) {
            for (e = s._next, r = i; r && r.pr > s.pr;) r = r._next;
            (s._prev = r ? r._prev : n) ? s._prev._next = s : i = s, (s._next = r) ? r._prev = s : n = s, s = e
        }
        t._pt = i
    }, pr = (_r.prototype.modifier = function (t, e, r) {
        this.mSet = this.mSet || this.set, this.set = tr, this.m = t, this.mt = r, this.tween = e
    }, _r);

    function _r(t, e, r, i, n, s, a, o, u) {
        this.t = e, this.s = i, this.c = n, this.p = r, this.r = s || sr, this.d = a || this, this.set = o || er, this.pr = u || 0, (this._next = t) && (t._prev = this)
    }

    y(ce + "parent,duration,ease,delay,overwrite,runBackwards,startAt,yoyo,immediateRender,repeat,repeatDelay,data,paused,reversed,lazy,callbackScope,stringFilter,id,yoyoEase,stagger,inherit,repeatRefresh,keyframes,autoRevert,scrollTrigger", (function (t) {
        return oe[t] = 1
    })), ae.TweenMax = ae.TweenLite = Je, ae.TimelineLite = ae.TimelineMax = Ne, mt = new Ne({
        sortChildren: !1,
        defaults: Nt,
        autoRemoveChildren: !0,
        id: "root",
        smoothChildTiming: !0
    }), Xt.stringFilter = ht;
    var cr = {
        registerPlugin: function () {
            for (var t = arguments.length, e = new Array(t), r = 0; r < t; r++) e[r] = arguments[r];
            e.forEach((function (t) {
                return function (t) {
                    var e = (t = !t.name && t.default || t).name, r = n(t), i = e && !r && t.init ? function () {
                            this._props = []
                        } : t, s = {init: d, render: ur, add: Ge, kill: fr, modifier: hr, rawVars: 0},
                        a = {targetTest: 0, get: 0, getSetter: nr, aliases: {}, register: 0};
                    if (Pe(), t !== i) {
                        if (fe[e]) return;
                        C(i, C(P(t, s), a)), de(i.prototype, de(s, P(t, a))), fe[i.prop = e] = i, t.targetTest && (_e.push(i), oe[e] = 1), e = ("css" === e ? "CSS" : e.charAt(0).toUpperCase() + e.substr(1)) + "Plugin"
                    }
                    c(e, i), t.register && t.register(gr, i, pr)
                }(t)
            }))
        },
        timeline: function (t) {
            return new Ne(t)
        },
        getTweensOf: function (t, e) {
            return mt.getTweensOf(t, e)
        },
        getProperty: function (t, e, r, n) {
            i(t) && (t = we(t)[0]);
            var s = g(t || {}).get, a = r ? A : k;
            return "native" === r && (r = ""), t ? e ? a((fe[e] && fe[e].get || s)(t, e, r, n)) : function (e, r, i) {
                return a((fe[e] && fe[e].get || s)(t, e, r, i))
            } : t
        },
        quickSetter: function (t, e, r) {
            if (1 < (t = we(t)).length) {
                var i = t.map((function (t) {
                    return gr.quickSetter(t, e, r)
                })), n = i.length;
                return function (t) {
                    for (var e = n; e--;) i[e](t)
                }
            }
            t = t[0] || {};
            var s = fe[e], a = g(t), o = a.harness && (a.harness.aliases || {})[e] || e, u = s ? function (e) {
                var i = new s;
                bt._pt = 0, i.init(t, r ? e + r : e, bt, 0, [t]), i.render(1, i), bt._pt && ur(1, bt)
            } : a.set(t, o);
            return s ? u : function (e) {
                return u(t, o, r ? e + r : e, a, 1)
            }
        },
        quickTo: function (t, e, r) {
            function i(t, r, i) {
                return s.resetTo(e, t, r, i)
            }

            var n, s = gr.to(t, de(((n = {})[e] = "+=0.1", n.paused = !0, n), r || {}));
            return i.tween = s, i
        },
        isTweening: function (t) {
            return 0 < mt.getTweensOf(t, !0).length
        },
        defaults: function (t) {
            return t && t.ease && (t.ease = Fe(t.ease, Nt.ease)), D(Nt, t || {})
        },
        config: function (t) {
            return D(Xt, t || {})
        },
        registerEffect: function (t) {
            var e = t.name, r = t.effect, i = t.plugins, n = t.defaults, s = t.extendTimeline;
            (i || "").split(",").forEach((function (t) {
                return t && !fe[t] && !ae[t] && _(e + " effect requires " + t + " plugin.")
            })), le[e] = function (t, e, i) {
                return r(we(t), C(e || {}, n), i)
            }, s && (Ne.prototype[e] = function (t, r, i) {
                return this.add(le[e](t, o(r) ? r : (i = r) && {}, this), i)
            })
        },
        registerEase: function (t, e) {
            Se[t] = Fe(e)
        },
        parseEase: function (t, e) {
            return arguments.length ? Fe(t, e) : Se
        },
        getById: function (t) {
            return mt.getById(t)
        },
        exportRoot: function (t, e) {
            void 0 === t && (t = {});
            var r, i, n = new Ne(t);
            for (n.smoothChildTiming = u(t.smoothChildTiming), mt.remove(n), n._dp = 0, n._time = n._tTime = mt._time, r = mt._first; r;) i = r._next, !e && !r._dur && r instanceof Je && r.vars.onComplete === r._targets[0] || X(n, r, r._start - r._delay), r = i;
            return X(mt, n, 0), n
        },
        utils: {
            wrap: function t(e, r, i) {
                var n = r - e;
                return Jt(e) ? et(e, t(0, e.length), r) : G(i, (function (t) {
                    return (n + (t - e) % n) % n + e
                }))
            }, wrapYoyo: function t(e, r, i) {
                var n = r - e, s = 2 * n;
                return Jt(e) ? et(e, t(0, e.length - 1), r) : G(i, (function (t) {
                    return e + (n < (t = (s + (t - e) % s) % s || 0) ? s - t : t)
                }))
            }, distribute: $, random: tt, snap: K, normalize: function (t, e, r) {
                return be(t, e, 0, 1, r)
            }, getUnit: W, clamp: function (t, e, r) {
                return G(r, (function (r) {
                    return Te(t, e, r)
                }))
            }, splitColor: at, toArray: we, selector: function (t) {
                return t = we(t)[0] || _("Invalid scope") || {}, function (e) {
                    var r = t.current || t.nativeElement || t;
                    return we(e, r.querySelectorAll ? r : r === t ? _("Invalid scope") || yt.createElement("div") : t)
                }
            }, mapRange: be, pipe: function () {
                for (var t = arguments.length, e = new Array(t), r = 0; r < t; r++) e[r] = arguments[r];
                return function (t) {
                    return e.reduce((function (t, e) {
                        return e(t)
                    }), t)
                }
            }, unitize: function (t, e) {
                return function (r) {
                    return t(parseFloat(r)) + (e || W(r))
                }
            }, interpolate: function t(e, r, n, s) {
                var a = isNaN(e + r) ? 0 : function (t) {
                    return (1 - t) * e + t * r
                };
                if (!a) {
                    var o, u, h, f, l, p = i(e), _ = {};
                    if (!0 === n && (s = 1) && (n = null), p) e = {p: e}, r = {p: r}; else if (Jt(e) && !Jt(r)) {
                        for (h = [], f = e.length, l = f - 2, u = 1; u < f; u++) h.push(t(e[u - 1], e[u]));
                        f--, a = function (t) {
                            t *= f;
                            var e = Math.min(l, ~~t);
                            return h[e](t - e)
                        }, n = r
                    } else s || (e = de(Jt(e) ? [] : {}, e));
                    if (!h) {
                        for (o in r) Ge.call(_, e, o, "get", r[o]);
                        a = function (t) {
                            return ur(t, _) || (p ? e.p : e)
                        }
                    }
                }
                return G(n, a)
            }, shuffle: Z
        },
        install: l,
        effects: le,
        ticker: De,
        updateRoot: Ne.updateRoot,
        plugins: fe,
        globalTimeline: mt,
        core: {
            PropTween: pr,
            globals: c,
            Tween: Je,
            Timeline: Ne,
            Animation: Ue,
            getCache: g,
            _removeLinkedListItem: R,
            suppressOverwrites: function (t) {
                return dt = t
            }
        }
    };

    function dr(t, e) {
        for (var r = t._pt; r && r.p !== e && r.op !== e && r.fp !== e;) r = r._next;
        return r
    }

    function mr(t, e) {
        return {
            name: t, rawVars: 1, init: function (t, r, n) {
                n._onInit = function (t) {
                    var n, s;
                    if (i(r) && (n = {}, y(r, (function (t) {
                        return n[t] = 1
                    })), r = n), e) {
                        for (s in n = {}, r) n[s] = e(r[s]);
                        r = n
                    }
                    !function (t, e) {
                        var r, i, n, s = t._targets;
                        for (r in e) for (i = s.length; i--;) (n = (n = t._ptLookup[i][r]) && n.d) && (n._pt && (n = dr(n, r)), n && n.modifier && n.modifier(e[r], t, s[i], r))
                    }(t, r)
                }
            }
        }
    }

    y("to,from,fromTo,delayedCall,set,killTweensOf", (function (t) {
        return cr[t] = Je[t]
    })), De.add(Ne.updateRoot), bt = cr.to({}, {duration: 0});
    var gr = cr.registerPlugin({
        name: "attr", init: function (t, e, r, i, n) {
            var s, a;
            for (s in e) (a = this.add(t, "setAttribute", (t.getAttribute(s) || 0) + "", e[s], i, n, 0, 0, s)) && (a.op = s), this._props.push(s)
        }
    }, {
        name: "endArray", init: function (t, e) {
            for (var r = e.length; r--;) this.add(t, r, t[r] || 0, e[r])
        }
    }, mr("roundProps", J), mr("modifiers"), mr("snap", K)) || cr;

    function vr(t, e) {
        return e.set(e.t, e.p, Math.round(1e4 * (e.s + e.c * t)) / 1e4 + e.u, e)
    }

    function yr(t, e) {
        return e.set(e.t, e.p, 1 === t ? e.e : Math.round(1e4 * (e.s + e.c * t)) / 1e4 + e.u, e)
    }

    function Tr(t, e) {
        return e.set(e.t, e.p, t ? Math.round(1e4 * (e.s + e.c * t)) / 1e4 + e.u : e.b, e)
    }

    function xr(t, e) {
        var r = e.s + e.c * t;
        e.set(e.t, e.p, ~~(r + (r < 0 ? -.5 : .5)) + e.u, e)
    }

    function wr(t, e) {
        return e.set(e.t, e.p, t ? e.e : e.b, e)
    }

    function br(t, e) {
        return e.set(e.t, e.p, 1 !== t ? e.b : e.e, e)
    }

    function Mr(t, e, r) {
        return t.style[e] = r
    }

    function Or(t, e, r) {
        return t.style.setProperty(e, r)
    }

    function kr(t, e, r) {
        return t._gsap[e] = r
    }

    function Ar(t, e, r) {
        return t._gsap.scaleX = t._gsap.scaleY = r
    }

    function Cr(t, e, r, i, n) {
        var s = t._gsap;
        s.scaleX = s.scaleY = r, s.renderTransform(n, s)
    }

    function Dr(t, e, r, i, n) {
        var s = t._gsap;
        s[e] = r, s.renderTransform(n, s)
    }

    function Pr(t, e) {
        var r = Kr.createElementNS ? Kr.createElementNS((e || "http://www.w3.org/1999/xhtml").replace(/^https/, "http"), t) : Kr.createElement(t);
        return r.style ? r : Kr.createElement(t)
    }

    function Sr(t, e, r) {
        var i = getComputedStyle(t);
        return i[e] || i.getPropertyValue(e.replace(Ai, "-$1").toLowerCase()) || i.getPropertyValue(e) || !r && Sr(t, Ei(e) || e, 1) || ""
    }

    function zr() {
        "undefined" != typeof window && window.document && (Jr = window, Kr = Jr.document, ti = Kr.documentElement, ri = Pr("div") || {style: {}}, Pr("div"), Si = Ei(Si), zi = Si + "Origin", ri.style.cssText = "border-width:0;line-height:0;position:absolute;padding:0", ni = !!Ei("perspective"), ei = 1)
    }

    function Rr(t) {
        var e,
            r = Pr("svg", this.ownerSVGElement && this.ownerSVGElement.getAttribute("xmlns") || "http://www.w3.org/2000/svg"),
            i = this.parentNode, n = this.nextSibling, s = this.style.cssText;
        if (ti.appendChild(r), r.appendChild(this), this.style.display = "block", t) try {
            e = this.getBBox(), this._gsapBBox = this.getBBox, this.getBBox = Rr
        } catch (t) {
        } else this._gsapBBox && (e = this._gsapBBox());
        return i && (n ? i.insertBefore(this, n) : i.appendChild(this)), ti.removeChild(r), this.style.cssText = s, e
    }

    function Er(t, e) {
        for (var r = e.length; r--;) if (t.hasAttribute(e[r])) return t.getAttribute(e[r])
    }

    function Fr(t) {
        var e;
        try {
            e = t.getBBox()
        } catch (r) {
            e = Rr.call(t, !0)
        }
        return e && (e.width || e.height) || t.getBBox === Rr || (e = Rr.call(t, !0)), !e || e.width || e.x || e.y ? e : {
            x: +Er(t, ["x", "cx", "x1"]) || 0,
            y: +Er(t, ["y", "cy", "y1"]) || 0,
            width: 0,
            height: 0
        }
    }

    function Br(t) {
        return !(!t.getCTM || t.parentNode && !t.ownerSVGElement || !Fr(t))
    }

    function Ir(t, e) {
        if (e) {
            var r = t.style;
            e in bi && e !== zi && (e = Si), r.removeProperty ? ("ms" !== e.substr(0, 2) && "webkit" !== e.substr(0, 6) || (e = "-" + e), r.removeProperty(e.replace(Ai, "-$1").toLowerCase())) : r.removeAttribute(e)
        }
    }

    function Lr(t, e, r, i, n, s) {
        var a = new pr(t._pt, e, r, 0, 1, s ? br : wr);
        return (t._pt = a).b = i, a.e = n, t._props.push(r), a
    }

    function Yr(t, e, r, i) {
        var n, s, a, o, u = parseFloat(r) || 0, h = (r + "").trim().substr((u + "").length) || "px", f = ri.style,
            l = Ci.test(e), p = "svg" === t.tagName.toLowerCase(),
            _ = (p ? "client" : "offset") + (l ? "Width" : "Height"), c = "px" === i, d = "%" === i;
        return i === h || !u || Fi[i] || Fi[h] ? u : ("px" === h || c || (u = Yr(t, e, r, "px")), o = t.getCTM && Br(t), !d && "%" !== h || !bi[e] && !~e.indexOf("adius") ? (f[l ? "width" : "height"] = 100 + (c ? h : i), s = ~e.indexOf("adius") || "em" === i && t.appendChild && !p ? t : t.parentNode, o && (s = (t.ownerSVGElement || {}).parentNode), s && s !== Kr && s.appendChild || (s = Kr.body), (a = s._gsap) && d && a.width && l && a.time === De.time ? T(u / a.width * 100) : (!d && "%" !== h || (f.position = Sr(t, "position")), s === t && (f.position = "static"), s.appendChild(ri), n = ri[_], s.removeChild(ri), f.position = "absolute", l && d && ((a = g(s)).time = De.time, a.width = s[_]), T(c ? n * u / 100 : n && u ? 100 / n * u : 0))) : (n = o ? t.getBBox()[l ? "width" : "height"] : t[_], T(d ? u / n * 100 : u / 100 * n)))
    }

    function Ur(t, e, r, i) {
        var n;
        return ei || zr(), e in Pi && "transform" !== e && ~(e = Pi[e]).indexOf(",") && (e = e.split(",")[0]), bi[e] && "transform" !== e ? (n = Ui(t, i), n = "transformOrigin" !== e ? n[e] : n.svg ? n.origin : Xi(Sr(t, zi)) + " " + n.zOrigin + "px") : (n = t.style[e]) && "auto" !== n && !i && !~(n + "").indexOf("calc(") || (n = Ii[e] && Ii[e](t, e, r) || Sr(t, e) || v(t, e) || ("opacity" === e ? 1 : 0)), r && !~(n + "").trim().indexOf(" ") ? Yr(t, e, n, r) + r : n
    }

    function Xr(t, e, r, i) {
        if (!r || "none" === r) {
            var n = Ei(e, t, 1), s = n && Sr(t, n, 1);
            s && s !== r ? (e = n, r = s) : "borderColor" === e && (r = Sr(t, "borderTopColor"))
        }
        var a, o, u, h, f, l, p, _, c, d, m, g = new pr(this._pt, t.style, e, 0, 1, or), v = 0, y = 0;
        if (g.b = r, g.e = i, r += "", "auto" == (i += "") && (t.style[e] = i, i = Sr(t, e) || i, t.style[e] = r), ht(a = [r, i]), i = a[1], u = (r = a[0]).match(ee) || [], (i.match(ee) || []).length) {
            for (; o = ee.exec(i);) p = o[0], c = i.substring(v, o.index), f ? f = (f + 1) % 5 : "rgba(" !== c.substr(-5) && "hsla(" !== c.substr(-5) || (f = 1), p !== (l = u[y++] || "") && (h = parseFloat(l) || 0, m = l.substr((h + "").length), "=" === p.charAt(1) && (p = w(h, p) + m), _ = parseFloat(p), d = p.substr((_ + "").length), v = ee.lastIndex - d.length, d || (d = d || Xt.units[e] || m, v === i.length && (i += d, g.e += d)), m !== d && (h = Yr(t, e, l, d) || 0), g._pt = {
                _next: g._pt,
                p: c || 1 === y ? c : ",",
                s: h,
                c: _ - h,
                m: f && f < 4 || "zIndex" === e ? Math.round : 0
            });
            g.c = v < i.length ? i.substring(v, i.length) : ""
        } else g.r = "display" === e && "none" === i ? br : wr;
        return ie.test(i) && (g.e = 0), this._pt = g
    }

    function Nr(t) {
        var e = t.split(" "), r = e[0], i = e[1] || "50%";
        return "top" !== r && "bottom" !== r && "left" !== i && "right" !== i || (t = r, r = i, i = t), e[0] = Bi[r] || r, e[1] = Bi[i] || i, e.join(" ")
    }

    function qr(t, e) {
        if (e.tween && e.tween._time === e.tween._dur) {
            var r, i, n, s = e.t, a = s.style, o = e.u, u = s._gsap;
            if ("all" === o || !0 === o) a.cssText = "", i = 1; else for (n = (o = o.split(",")).length; -1 < --n;) r = o[n], bi[r] && (i = 1, r = "transformOrigin" === r ? zi : Si), Ir(s, r);
            i && (Ir(s, Si), u && (u.svg && s.removeAttribute("transform"), Ui(s, 1), u.uncache = 1))
        }
    }

    function Vr(t) {
        return "matrix(1, 0, 0, 1, 0, 0)" === t || "none" === t || !t
    }

    function jr(t) {
        var e = Sr(t, Si);
        return Vr(e) ? Li : e.substr(7).match(te).map(T)
    }

    function Qr(t, e) {
        var r, i, n, s, a = t._gsap || g(t), o = t.style, u = jr(t);
        return a.svg && t.getAttribute("transform") ? "1,0,0,1,0,0" === (u = [(n = t.transform.baseVal.consolidate().matrix).a, n.b, n.c, n.d, n.e, n.f]).join(",") ? Li : u : (u !== Li || t.offsetParent || t === ti || a.svg || (n = o.display, o.display = "block", (r = t.parentNode) && t.offsetParent || (s = 1, i = t.nextSibling, ti.appendChild(t)), u = jr(t), n ? o.display = n : Ir(t, "display"), s && (i ? r.insertBefore(t, i) : r ? r.appendChild(t) : ti.removeChild(t))), e && 6 < u.length ? [u[0], u[1], u[4], u[5], u[12], u[13]] : u)
    }

    function Gr(t, e, r, i, n, s) {
        var a, o, u, h = t._gsap, f = n || Qr(t, !0), l = h.xOrigin || 0, p = h.yOrigin || 0, _ = h.xOffset || 0,
            c = h.yOffset || 0, d = f[0], m = f[1], g = f[2], v = f[3], y = f[4], T = f[5], x = e.split(" "),
            w = parseFloat(x[0]) || 0, b = parseFloat(x[1]) || 0;
        r ? f !== Li && (o = d * v - m * g) && (u = w * (-m / o) + b * (d / o) - (d * T - m * y) / o, w = w * (v / o) + b * (-g / o) + (g * T - v * y) / o, b = u) : (w = (a = Fr(t)).x + (~x[0].indexOf("%") ? w / 100 * a.width : w), b = a.y + (~(x[1] || x[0]).indexOf("%") ? b / 100 * a.height : b)), i || !1 !== i && h.smooth ? (y = w - l, T = b - p, h.xOffset = _ + (y * d + T * g) - y, h.yOffset = c + (y * m + T * v) - T) : h.xOffset = h.yOffset = 0, h.xOrigin = w, h.yOrigin = b, h.smooth = !!i, h.origin = e, h.originIsAbsolute = !!r, t.style[zi] = "0px 0px", s && (Lr(s, h, "xOrigin", l, w), Lr(s, h, "yOrigin", p, b), Lr(s, h, "xOffset", _, h.xOffset), Lr(s, h, "yOffset", c, h.yOffset)), t.setAttribute("data-svg-origin", w + " " + b)
    }

    function Wr(t, e, r) {
        var i = W(e);
        return T(parseFloat(e) + parseFloat(Yr(t, "x", r + "px", i))) + i
    }

    function Hr(t, e, r, n, s) {
        var a, o, u = 360, h = i(s), f = parseFloat(s) * (h && ~s.indexOf("rad") ? Mi : 1) - n, l = n + f + "deg";
        return h && ("short" === (a = s.split("_")[1]) && (f %= u) != f % 180 && (f += f < 0 ? u : -u), "cw" === a && f < 0 ? f = (f + 36e9) % u - ~~(f / u) * u : "ccw" === a && 0 < f && (f = (f - 36e9) % u - ~~(f / u) * u)), t._pt = o = new pr(t._pt, e, r, n, f, yr), o.e = l, o.u = "deg", t._props.push(r), o
    }

    function Zr(t, e) {
        for (var r in e) t[r] = e[r];
        return t
    }

    function $r(t, e, r) {
        var i, n, s, a, o, u, h, f = Zr({}, r._gsap), l = r.style;
        for (n in f.svg ? (s = r.getAttribute("transform"), r.setAttribute("transform", ""), l[Si] = e, i = Ui(r, 1), Ir(r, Si), r.setAttribute("transform", s)) : (s = getComputedStyle(r)[Si], l[Si] = e, i = Ui(r, 1), l[Si] = s), bi) (s = f[n]) !== (a = i[n]) && "perspective,force3D,transformOrigin,svgOrigin".indexOf(n) < 0 && (o = W(s) !== (h = W(a)) ? Yr(r, n, s, h) : parseFloat(s), u = parseFloat(a), t._pt = new pr(t._pt, i, n, o, u - o, vr), t._pt.u = h || 0, t._props.push(n));
        Zr(i, f)
    }

    Je.version = Ne.version = gr.version = "3.10.4", xt = 1, h() && Pe();
    var Jr, Kr, ti, ei, ri, ii, ni, si = Se.Power0, ai = Se.Power1, oi = Se.Power2, ui = Se.Power3, hi = Se.Power4,
        fi = Se.Linear, li = Se.Quad, pi = Se.Cubic, _i = Se.Quart, ci = Se.Quint, di = Se.Strong, mi = Se.Elastic,
        gi = Se.Back, vi = Se.SteppedEase, yi = Se.Bounce, Ti = Se.Sine, xi = Se.Expo, wi = Se.Circ, bi = {},
        Mi = 180 / Math.PI, Oi = Math.PI / 180, ki = Math.atan2, Ai = /([A-Z])/g,
        Ci = /(left|right|width|margin|padding|x)/i, Di = /[\s,\(]\S/,
        Pi = {autoAlpha: "opacity,visibility", scale: "scaleX,scaleY", alpha: "opacity"}, Si = "transform",
        zi = Si + "Origin", Ri = "O,Moz,ms,Ms,Webkit".split(","), Ei = function (t, e, r) {
            var i = (e || ri).style, n = 5;
            if (t in i && !r) return t;
            for (t = t.charAt(0).toUpperCase() + t.substr(1); n-- && !(Ri[n] + t in i);) ;
            return n < 0 ? null : (3 === n ? "ms" : 0 <= n ? Ri[n] : "") + t
        }, Fi = {deg: 1, rad: 1, turn: 1}, Bi = {top: "0%", bottom: "100%", left: "0%", right: "100%", center: "50%"},
        Ii = {
            clearProps: function (t, e, r, i, n) {
                if ("isFromStart" !== n.data) {
                    var s = t._pt = new pr(t._pt, e, r, 0, 0, qr);
                    return s.u = i, s.pr = -10, s.tween = n, t._props.push(r), 1
                }
            }
        }, Li = [1, 0, 0, 1, 0, 0], Yi = {}, Ui = function (t, e) {
            var r = t._gsap || new Ye(t);
            if ("x" in r && !e && !r.uncache) return r;
            var i, n, s, a, o, u, h, f, l, p, _, c, d, m, g, v, y, x, w, b, M, O, k, A, C, D, P, S, z, R, E, F, B = t.style,
                I = r.scaleX < 0, L = "deg", Y = Sr(t, zi) || "0";
            return i = n = s = u = h = f = l = p = _ = 0, a = o = 1, r.svg = !(!t.getCTM || !Br(t)), m = Qr(t, r.svg), r.svg && (A = (!r.uncache || "0px 0px" === Y) && !e && t.getAttribute("data-svg-origin"), Gr(t, A || Y, !!A || r.originIsAbsolute, !1 !== r.smooth, m)), c = r.xOrigin || 0, d = r.yOrigin || 0, m !== Li && (x = m[0], w = m[1], b = m[2], M = m[3], i = O = m[4], n = k = m[5], 6 === m.length ? (a = Math.sqrt(x * x + w * w), o = Math.sqrt(M * M + b * b), u = x || w ? ki(w, x) * Mi : 0, (l = b || M ? ki(b, M) * Mi + u : 0) && (o *= Math.abs(Math.cos(l * Oi))), r.svg && (i -= c - (c * x + d * b), n -= d - (c * w + d * M))) : (F = m[6], R = m[7], P = m[8], S = m[9], z = m[10], E = m[11], i = m[12], n = m[13], s = m[14], h = (g = ki(F, z)) * Mi, g && (A = O * (v = Math.cos(-g)) + P * (y = Math.sin(-g)), C = k * v + S * y, D = F * v + z * y, P = O * -y + P * v, S = k * -y + S * v, z = F * -y + z * v, E = R * -y + E * v, O = A, k = C, F = D), f = (g = ki(-b, z)) * Mi, g && (v = Math.cos(-g), E = M * (y = Math.sin(-g)) + E * v, x = A = x * v - P * y, w = C = w * v - S * y, b = D = b * v - z * y), u = (g = ki(w, x)) * Mi, g && (A = x * (v = Math.cos(g)) + w * (y = Math.sin(g)), C = O * v + k * y, w = w * v - x * y, k = k * v - O * y, x = A, O = C), h && 359.9 < Math.abs(h) + Math.abs(u) && (h = u = 0, f = 180 - f), a = T(Math.sqrt(x * x + w * w + b * b)), o = T(Math.sqrt(k * k + F * F)), g = ki(O, k), l = 2e-4 < Math.abs(g) ? g * Mi : 0, _ = E ? 1 / (E < 0 ? -E : E) : 0), r.svg && (A = t.getAttribute("transform"), r.forceCSS = t.setAttribute("transform", "") || !Vr(Sr(t, Si)), A && t.setAttribute("transform", A))), 90 < Math.abs(l) && Math.abs(l) < 270 && (I ? (a *= -1, l += u <= 0 ? 180 : -180, u += u <= 0 ? 180 : -180) : (o *= -1, l += l <= 0 ? 180 : -180)), e = e || r.uncache, r.x = i - ((r.xPercent = i && (!e && r.xPercent || (Math.round(t.offsetWidth / 2) === Math.round(-i) ? -50 : 0))) ? t.offsetWidth * r.xPercent / 100 : 0) + "px", r.y = n - ((r.yPercent = n && (!e && r.yPercent || (Math.round(t.offsetHeight / 2) === Math.round(-n) ? -50 : 0))) ? t.offsetHeight * r.yPercent / 100 : 0) + "px", r.z = s + "px", r.scaleX = T(a), r.scaleY = T(o), r.rotation = T(u) + L, r.rotationX = T(h) + L, r.rotationY = T(f) + L, r.skewX = l + L, r.skewY = p + L, r.transformPerspective = _ + "px", (r.zOrigin = parseFloat(Y.split(" ")[2]) || 0) && (B[zi] = Xi(Y)), r.xOffset = r.yOffset = 0, r.force3D = Xt.force3D, r.renderTransform = r.svg ? Gi : ni ? Qi : Ni, r.uncache = 0, r
        }, Xi = function (t) {
            return (t = t.split(" "))[0] + " " + t[1]
        }, Ni = function (t, e) {
            e.z = "0px", e.rotationY = e.rotationX = "0deg", e.force3D = 0, Qi(t, e)
        }, qi = "0deg", Vi = "0px", ji = ") ", Qi = function (t, e) {
            var r = e || this, i = r.xPercent, n = r.yPercent, s = r.x, a = r.y, o = r.z, u = r.rotation, h = r.rotationY,
                f = r.rotationX, l = r.skewX, p = r.skewY, _ = r.scaleX, c = r.scaleY, d = r.transformPerspective,
                m = r.force3D, g = r.target, v = r.zOrigin, y = "", T = "auto" === m && t && 1 !== t || !0 === m;
            if (v && (f !== qi || h !== qi)) {
                var x, w = parseFloat(h) * Oi, b = Math.sin(w), M = Math.cos(w);
                w = parseFloat(f) * Oi, s = Wr(g, s, b * (x = Math.cos(w)) * -v), a = Wr(g, a, -Math.sin(w) * -v), o = Wr(g, o, M * x * -v + v)
            }
            d !== Vi && (y += "perspective(" + d + ji), (i || n) && (y += "translate(" + i + "%, " + n + "%) "), !T && s === Vi && a === Vi && o === Vi || (y += o !== Vi || T ? "translate3d(" + s + ", " + a + ", " + o + ") " : "translate(" + s + ", " + a + ji), u !== qi && (y += "rotate(" + u + ji), h !== qi && (y += "rotateY(" + h + ji), f !== qi && (y += "rotateX(" + f + ji), l === qi && p === qi || (y += "skew(" + l + ", " + p + ji), 1 === _ && 1 === c || (y += "scale(" + _ + ", " + c + ji), g.style[Si] = y || "translate(0, 0)"
        }, Gi = function (t, e) {
            var r, i, n, s, a, o = e || this, u = o.xPercent, h = o.yPercent, f = o.x, l = o.y, p = o.rotation, _ = o.skewX,
                c = o.skewY, d = o.scaleX, m = o.scaleY, g = o.target, v = o.xOrigin, y = o.yOrigin, x = o.xOffset,
                w = o.yOffset, b = o.forceCSS, M = parseFloat(f), O = parseFloat(l);
            p = parseFloat(p), _ = parseFloat(_), (c = parseFloat(c)) && (_ += c = parseFloat(c), p += c), p || _ ? (p *= Oi, _ *= Oi, r = Math.cos(p) * d, i = Math.sin(p) * d, n = Math.sin(p - _) * -m, s = Math.cos(p - _) * m, _ && (c *= Oi, a = Math.tan(_ - c), n *= a = Math.sqrt(1 + a * a), s *= a, c && (a = Math.tan(c), r *= a = Math.sqrt(1 + a * a), i *= a)), r = T(r), i = T(i), n = T(n), s = T(s)) : (r = d, s = m, i = n = 0), (M && !~(f + "").indexOf("px") || O && !~(l + "").indexOf("px")) && (M = Yr(g, "x", f, "px"), O = Yr(g, "y", l, "px")), (v || y || x || w) && (M = T(M + v - (v * r + y * n) + x), O = T(O + y - (v * i + y * s) + w)), (u || h) && (M = T(M + u / 100 * (a = g.getBBox()).width), O = T(O + h / 100 * a.height)), a = "matrix(" + r + "," + i + "," + n + "," + s + "," + M + "," + O + ")", g.setAttribute("transform", a), b && (g.style[Si] = a)
        };
    y("padding,margin,Width,Radius", (function (t, e) {
        var r = "Right", i = "Bottom", n = "Left",
            s = (e < 3 ? ["Top", r, i, n] : ["Top" + n, "Top" + r, i + r, i + n]).map((function (r) {
                return e < 2 ? t + r : "border" + r + t
            }));
        Ii[1 < e ? "border" + t : t] = function (t, e, r, i, n) {
            var a, o;
            if (arguments.length < 4) return a = s.map((function (e) {
                return Ur(t, e, r)
            })), 5 === (o = a.join(" ")).split(a[0]).length ? a[0] : o;
            a = (i + "").split(" "), o = {}, s.forEach((function (t, e) {
                return o[t] = a[e] = a[e] || a[(e - 1) / 2 | 0]
            })), t.init(e, o, n)
        }
    }));
    var Wi, Hi, Zi = {
        name: "css", register: zr, targetTest: function (t) {
            return t.style && t.nodeType
        }, init: function (t, e, r, n, s) {
            var a, o, u, h, f, l, _, c, d, m, g, v, y, T, x, b = this._props, M = t.style, O = r.vars.startAt;
            for (_ in ei || zr(), e) if ("autoRound" !== _ && (o = e[_], !fe[_] || !qe(_, e, r, n, t, s))) if (f = typeof o, l = Ii[_], "function" === f && (f = typeof (o = o.call(r, n, t, s))), "string" === f && ~o.indexOf("random(") && (o = rt(o)), l) l(this, t, _, o, r) && (x = 1); else if ("--" === _.substr(0, 2)) a = (getComputedStyle(t).getPropertyValue(_) + "").trim(), o += "", Ae.lastIndex = 0, Ae.test(a) || (c = W(a), d = W(o)), d ? c !== d && (a = Yr(t, _, a, d) + d) : c && (o += c), this.add(M, "setProperty", a, o, n, s, 0, 0, _), b.push(_); else if ("undefined" !== f) {
                if (O && _ in O ? (i(a = "function" == typeof O[_] ? O[_].call(r, n, t, s) : O[_]) && ~a.indexOf("random(") && (a = rt(a)), W(a + "") || (a += Xt.units[_] || W(Ur(t, _)) || ""), "=" === (a + "").charAt(1) && (a = Ur(t, _))) : a = Ur(t, _), h = parseFloat(a), (m = "string" === f && "=" === o.charAt(1) && o.substr(0, 2)) && (o = o.substr(2)), u = parseFloat(o), _ in Pi && ("autoAlpha" === _ && (1 === h && "hidden" === Ur(t, "visibility") && u && (h = 0), Lr(this, M, "visibility", h ? "inherit" : "hidden", u ? "inherit" : "hidden", !u)), "scale" !== _ && "transform" !== _ && ~(_ = Pi[_]).indexOf(",") && (_ = _.split(",")[0])), g = _ in bi) if (v || ((y = t._gsap).renderTransform && !e.parseTransform || Ui(t, e.parseTransform), T = !1 !== e.smoothOrigin && y.smooth, (v = this._pt = new pr(this._pt, M, Si, 0, 1, y.renderTransform, y, 0, -1)).dep = 1), "scale" === _) this._pt = new pr(this._pt, y, "scaleY", y.scaleY, (m ? w(y.scaleY, m + u) : u) - y.scaleY || 0), b.push("scaleY", _), _ += "X"; else {
                    if ("transformOrigin" === _) {
                        o = Nr(o), y.svg ? Gr(t, o, 0, T, 0, this) : ((d = parseFloat(o.split(" ")[2]) || 0) !== y.zOrigin && Lr(this, y, "zOrigin", y.zOrigin, d), Lr(this, M, _, Xi(a), Xi(o)));
                        continue
                    }
                    if ("svgOrigin" === _) {
                        Gr(t, o, 1, T, 0, this);
                        continue
                    }
                    if (_ in Yi) {
                        Hr(this, y, _, h, m ? w(h, m + o) : o);
                        continue
                    }
                    if ("smoothOrigin" === _) {
                        Lr(this, y, "smooth", y.smooth, o);
                        continue
                    }
                    if ("force3D" === _) {
                        y[_] = o;
                        continue
                    }
                    if ("transform" === _) {
                        $r(this, o, t);
                        continue
                    }
                } else _ in M || (_ = Ei(_) || _);
                if (g || (u || 0 === u) && (h || 0 === h) && !Di.test(o) && _ in M) u = u || 0, (c = (a + "").substr((h + "").length)) !== (d = W(o) || (_ in Xt.units ? Xt.units[_] : c)) && (h = Yr(t, _, a, d)), this._pt = new pr(this._pt, g ? y : M, _, h, (m ? w(h, m + u) : u) - h, g || "px" !== d && "zIndex" !== _ || !1 === e.autoRound ? vr : xr), this._pt.u = d || 0, c !== d && "%" !== d && (this._pt.b = a, this._pt.r = Tr); else if (_ in M) Xr.call(this, t, _, a, m ? m + o : o); else {
                    if (!(_ in t)) {
                        p(_, o);
                        continue
                    }
                    this.add(t, _, a || t[_], m ? m + o : o, n, s)
                }
                b.push(_)
            }
            x && lr(this)
        }, get: Ur, aliases: Pi, getSetter: function (t, e, r) {
            var i = Pi[e];
            return i && i.indexOf(",") < 0 && (e = i), e in bi && e !== zi && (t._gsap.x || Ur(t, "x")) ? r && ii === r ? "scale" === e ? Ar : kr : (ii = r || {}) && ("scale" === e ? Cr : Dr) : t.style && !a(t.style[e]) ? Mr : ~e.indexOf("-") ? Or : nr(t, e)
        }, core: {_removeProperty: Ir, _getMatrix: Qr}
    };
    gr.utils.checkPrefix = Ei, Hi = y("x,y,z,scale,scaleX,scaleY,xPercent,yPercent" + "," + (Wi = "rotation,rotationX,rotationY,skewX,skewY") + ",transform,transformOrigin,svgOrigin,force3D,smoothOrigin,transformPerspective", (function (t) {
        bi[t] = 1
    })), y(Wi, (function (t) {
        Xt.units[t] = "deg", Yi[t] = 1
    })), Pi[Hi[13]] = "x,y,z,scale,scaleX,scaleY,xPercent,yPercent," + Wi, y("0:translateX,1:translateY,2:translateZ,8:rotate,8:rotationZ,8:rotateZ,9:rotateX,10:rotateY", (function (t) {
        var e = t.split(":");
        Pi[e[1]] = Hi[e[0]]
    })), y("x,y,z,top,right,bottom,left,width,height,fontSize,padding,margin,perspective", (function (t) {
        Xt.units[t] = "px"
    })), gr.registerPlugin(Zi);
    var $i = gr.registerPlugin(Zi) || gr, Ji = $i.core.Tween;
    t.Back = gi, t.Bounce = yi, t.CSSPlugin = Zi, t.Circ = wi, t.Cubic = pi, t.Elastic = mi, t.Expo = xi, t.Linear = fi, t.Power0 = si, t.Power1 = ai, t.Power2 = oi, t.Power3 = ui, t.Power4 = hi, t.Quad = li, t.Quart = _i, t.Quint = ci, t.Sine = Ti, t.SteppedEase = vi, t.Strong = di, t.TimelineLite = Ne, t.TimelineMax = Ne, t.TweenLite = Je, t.TweenMax = Ji, t.default = $i, t.gsap = $i, "undefined" == typeof window || window !== t ? Object.defineProperty(t, "__esModule", {value: !0}) : delete t.default
}));/*!
 * ScrollTrigger 3.10.4
 * https://greensock.com
 * 
 * @license Copyright 2022, GreenSock. All rights reserved.
 * Subject to the terms at https://greensock.com/standard-license or for Club GreenSock members, the agreement issued with that membership.
 * @author: Jack Doyle, jack@greensock.com
 */
!function (e, t) {
    "object" == typeof exports && "undefined" != typeof module ? t(exports) : "function" == typeof define && define.amd ? define(["exports"], t) : t((e = e || self).window = e.window || {})
}(this, (function (e) {
    "use strict";

    function t(e, t) {
        for (var n = 0; n < t.length; n++) {
            var r = t[n];
            r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
        }
    }

    function n() {
        return v || "undefined" != typeof window && (v = window.gsap) && v.registerPlugin && v
    }

    function r(e, t) {
        return ~A.indexOf(e) && A[A.indexOf(e) + 1][t]
    }

    function i(e) {
        return !!~T.indexOf(e)
    }

    function o(e, t, n, r, i) {
        return e.addEventListener(t, n, {passive: !r, capture: !!i})
    }

    function a(e, t, n, r) {
        return e.removeEventListener(t, n, !!r)
    }

    function s() {
        return M && M.isPressed || O.cache++
    }

    function l(e, t) {
        function n(r) {
            if (r || 0 === r) {
                P && (y.history.scrollRestoration = "manual");
                var i = M && M.isPressed;
                r = n.v = Math.round(r) || (M && M.iOS ? 1 : 0), e(r), n.cacheID = O.cache, i && Y("ss", r)
            } else (t || O.cache !== n.cacheID || Y("ref")) && (n.cacheID = O.cache, n.v = e());
            return n.v + n.offset
        }

        return n.offset = 0, e && n
    }

    function c(e) {
        return v.utils.toArray(e)[0] || ("string" == typeof e && !1 !== v.config().nullTargetWarn ? console.warn("Element not found:", e) : null)
    }

    function u(e, t) {
        var n = t.s, o = t.sc, a = O.indexOf(e), s = o === I.sc ? 1 : 2;
        return ~a || (a = O.push(e) - 1), O[a + s] || (O[a + s] = l(r(e, n), !0) || (i(e) ? o : l((function (t) {
            return arguments.length ? e[n] = t : e[n]
        }))))
    }

    function f(e, t, n) {
        function r(e, t) {
            var r = D();
            t || l < r - a ? (o = i, i = e, s = a, a = r) : n ? i += e : i = o + (e - o) / (r - s) * (a - s)
        }

        var i = e, o = e, a = D(), s = a, l = t || 50, c = Math.max(500, 3 * l);
        return {
            update: r, reset: function () {
                o = i = n ? 0 : i, s = a = 0
            }, getVelocity: function (e) {
                var t = s, l = o, u = D();
                return !e && 0 !== e || e === i || r(e), a === s || c < u - s ? 0 : (i + (n ? l : -l)) / ((n ? u : a) - t) * 1e3
            }
        }
    }

    function d(e, t) {
        return t && !e._gsapAllow && e.preventDefault(), e.changedTouches ? e.changedTouches[0] : e
    }

    function p(e) {
        var t = Math.max.apply(Math, e), n = Math.min.apply(Math, e);
        return Math.abs(t) >= Math.abs(n) ? t : n
    }

    function h() {
        (k = v.core.globals().ScrollTrigger) && k.core && function () {
            var e = k.core, t = e.bridge || {}, n = e._scrollers, r = e._proxies;
            n.push.apply(n, O), r.push.apply(r, A), O = n, A = r, Y = function (e, n) {
                return t[e](n)
            }
        }()
    }

    function g(e) {
        return (v = e || n()) && "undefined" != typeof document && document.body && (y = window, x = (b = document).documentElement, w = b.body, T = [y, b, x, w], v.utils.clamp, S = "onpointerenter" in w ? "pointer" : "mouse", _ = B.isTouch = y.matchMedia && y.matchMedia("(hover: none), (pointer: coarse)").matches ? 1 : "ontouchstart" in y || 0 < navigator.maxTouchPoints || 0 < navigator.msMaxTouchPoints ? 2 : 0, E = B.eventTypes = ("ontouchstart" in x ? "touchstart,touchmove,touchcancel,touchend" : "onpointerdown" in x ? "pointerdown,pointermove,pointercancel,pointerup" : "mousedown,mousemove,mouseup,mouseup").split(","), setTimeout((function () {
            return P = 0
        }), 500), h(), m = 1), m
    }

    var v, m, y, b, x, w, _, S, k, T, M, E, P = 1, C = [], O = [], A = [], D = Date.now, Y = function (e, t) {
        return t
    }, R = "scrollLeft", X = "scrollTop", z = {
        s: R,
        p: "left",
        p2: "Left",
        os: "right",
        os2: "Right",
        d: "width",
        d2: "Width",
        a: "x",
        sc: l((function (e) {
            return arguments.length ? y.scrollTo(e, I.sc()) : y.pageXOffset || b[R] || x[R] || w[R] || 0
        }))
    }, I = {
        s: X,
        p: "top",
        p2: "Top",
        os: "bottom",
        os2: "Bottom",
        d: "height",
        d2: "Height",
        a: "y",
        op: z,
        sc: l((function (e) {
            return arguments.length ? y.scrollTo(z.sc(), e) : y.pageYOffset || b[X] || x[X] || w[X] || 0
        }))
    };
    z.op = I, O.cache = 0;
    var B = (H.prototype.init = function (e) {
        m || g(v) || console.warn("Please gsap.registerPlugin(Observer)"), k || h();
        var t = e.tolerance, n = e.dragMinimum, r = e.type, l = e.target, T = e.lineHeight, P = e.debounce,
            O = e.preventDefault, A = e.onStop, Y = e.onStopDelay, R = e.ignore, X = e.wheelSpeed, B = e.event,
            H = e.onDragStart, L = e.onDragEnd, N = e.onDrag, W = e.onPress, F = e.onRelease, V = e.onRight,
            q = e.onLeft, G = e.onUp, U = e.onDown, j = e.onChangeX, K = e.onChangeY, Z = e.onChange, $ = e.onToggleX,
            J = e.onToggleY, Q = e.onHover, ee = e.onHoverEnd, te = e.onMove, ne = e.ignoreCheck, re = e.isNormalizer,
            ie = e.onGestureStart, oe = e.onGestureEnd, ae = e.onWheel, se = e.onEnable, le = e.onDisable,
            ce = e.onClick, ue = e.scrollSpeed, fe = e.capture, de = e.allowClicks, pe = e.lockAxis, he = e.onLockAxis;

        function ge() {
            return Ke = D()
        }

        function ve(e, t) {
            return (Ie.event = e) && R && ~R.indexOf(e.target) || t && Ve && "touch" !== e.pointerType || ne && ne(e, t)
        }

        function me() {
            var e = Ie.deltaX = p(Ue), n = Ie.deltaY = p(je), r = Math.abs(e) >= t, i = Math.abs(n) >= t;
            Z && (r || i) && Z(Ie, e, n, Ue, je), r && (V && 0 < Ie.deltaX && V(Ie), q && Ie.deltaX < 0 && q(Ie), j && j(Ie), $ && Ie.deltaX < 0 != Be < 0 && $(Ie), Be = Ie.deltaX, Ue[0] = Ue[1] = Ue[2] = 0), i && (U && 0 < Ie.deltaY && U(Ie), G && Ie.deltaY < 0 && G(Ie), K && K(Ie), J && Ie.deltaY < 0 != He < 0 && J(Ie), He = Ie.deltaY, je[0] = je[1] = je[2] = 0), (Ye || De) && (te && te(Ie), he && Xe && he(Ie), De && (N(Ie), De = !1), Ye = Xe = !1), Re && (ae(Ie), Re = !1), Oe = 0
        }

        function ye(e, t, n) {
            Ue[n] += e, je[n] += t, Ie._vx.update(e), Ie._vy.update(t), P ? Oe = Oe || requestAnimationFrame(me) : me()
        }

        function be(e, t) {
            "y" !== ze && (Ue[2] += e, Ie._vx.update(e, !0)), "x" !== ze && (je[2] += t, Ie._vy.update(t, !0)), pe && !ze && (Ie.axis = ze = Math.abs(e) > Math.abs(t) ? "x" : "y", Xe = !0), P ? Oe = Oe || requestAnimationFrame(me) : me()
        }

        function xe(e) {
            if (!ve(e, 1)) {
                var t = (e = d(e, O)).clientX, r = e.clientY, i = t - Ie.x, o = r - Ie.y, a = Ie.isDragging;
                Ie.x = t, Ie.y = r, (a || Math.abs(Ie.startX - t) >= n || Math.abs(Ie.startY - r) >= n) && (N && (De = !0), a || (Ie.isDragging = !0), be(i, o), a || H && H(Ie))
            }
        }

        function we(e) {
            if (!ve(e, 1)) {
                a(re ? l : Ge, E[1], xe, !0);
                var t = Ie.isDragging && (3 < Math.abs(Ie.x - Ie.startX) || 3 < Math.abs(Ie.y - Ie.startY)), n = d(e);
                t || (Ie._vx.reset(), Ie._vy.reset(), O && de && v.delayedCall(.08, (function () {
                    if (300 < D() - Ke && !e.defaultPrevented) if (e.target.click) e.target.click(); else if (Ge.createEvent) {
                        var t = Ge.createEvent("MouseEvents");
                        t.initMouseEvent("click", !0, !0, y, 1, n.screenX, n.screenY, n.clientX, n.clientY, !1, !1, !1, !1, 0, null), e.target.dispatchEvent(t)
                    }
                }))), Ie.isDragging = Ie.isGesturing = Ie.isPressed = !1, A && !re && Ae.restart(!0), L && t && L(Ie), F && F(Ie, t)
            }
        }

        function _e(e) {
            return e.touches && 1 < e.touches.length && (Ie.isGesturing = !0) && ie(e, Ie.isDragging)
        }

        function Se() {
            return (Ie.isGesturing = !1) || oe(Ie)
        }

        function ke(e) {
            if (!ve(e)) {
                var t = Le(), n = Ne();
                ye((t - We) * ue, (n - Fe) * ue, 1), We = t, Fe = n, A && Ae.restart(!0)
            }
        }

        function Te(e) {
            if (!ve(e)) {
                e = d(e, O), ae && (Re = !0);
                var t = (1 === e.deltaMode ? T : 2 === e.deltaMode ? y.innerHeight : 1) * X;
                ye(e.deltaX * t, e.deltaY * t, 0), A && !re && Ae.restart(!0)
            }
        }

        function Me(e) {
            if (!ve(e)) {
                var t = e.clientX, n = e.clientY, r = t - Ie.x, i = n - Ie.y;
                Ie.x = t, Ie.y = n, Ye = !0, (r || i) && be(r, i)
            }
        }

        function Ee(e) {
            Ie.event = e, Q(Ie)
        }

        function Pe(e) {
            Ie.event = e, ee(Ie)
        }

        function Ce(e) {
            return ve(e) || d(e, O) && ce(Ie)
        }

        this.target = l = c(l) || x, this.vars = e, R = R && v.utils.toArray(R), t = t || 0, n = n || 0, X = X || 1, ue = ue || 1, r = r || "wheel,touch,pointer", P = !1 !== P, T = T || parseFloat(y.getComputedStyle(w).lineHeight) || 22;
        var Oe, Ae, De, Ye, Re, Xe, ze, Ie = this, Be = 0, He = 0, Le = u(l, z), Ne = u(l, I), We = Le(), Fe = Ne(),
            Ve = ~r.indexOf("touch") && !~r.indexOf("pointer") && "pointerdown" === E[0], qe = i(l),
            Ge = l.ownerDocument || b, Ue = [0, 0, 0], je = [0, 0, 0], Ke = 0, Ze = Ie.onPress = function (e) {
                ve(e, 1) || (Ie.axis = ze = null, Ae.pause(), Ie.isPressed = !0, e = d(e), Be = He = 0, Ie.startX = Ie.x = e.clientX, Ie.startY = Ie.y = e.clientY, Ie._vx.reset(), Ie._vy.reset(), o(re ? l : Ge, E[1], xe, O, !0), Ie.deltaX = Ie.deltaY = 0, W && W(Ie))
            };
        Ae = Ie._dc = v.delayedCall(Y || .25, (function () {
            Ie._vx.reset(), Ie._vy.reset(), Ae.pause(), A && A(Ie)
        })).pause(), Ie.deltaX = Ie.deltaY = 0, Ie._vx = f(0, 50, !0), Ie._vy = f(0, 50, !0), Ie.scrollX = Le, Ie.scrollY = Ne, Ie.isDragging = Ie.isGesturing = Ie.isPressed = !1, Ie.enable = function (e) {
            return Ie.isEnabled || (o(qe ? Ge : l, "scroll", s), 0 <= r.indexOf("scroll") && o(qe ? Ge : l, "scroll", ke, O, fe), 0 <= r.indexOf("wheel") && o(l, "wheel", Te, O, fe), (0 <= r.indexOf("touch") && _ || 0 <= r.indexOf("pointer")) && (o(l, E[0], Ze, O, fe), o(Ge, E[2], we), o(Ge, E[3], we), de && o(l, "click", ge, !1, !0), ce && o(l, "click", Ce), ie && o(Ge, "gesturestart", _e), oe && o(Ge, "gestureend", Se), Q && o(l, S + "enter", Ee), ee && o(l, S + "leave", Pe), te && o(l, S + "move", Me)), Ie.isEnabled = !0, e && e.type && Ze(e), se && se(Ie)), Ie
        }, Ie.disable = function () {
            Ie.isEnabled && (C.filter((function (e) {
                return e !== Ie && i(e.target)
            })).length || a(qe ? Ge : l, "scroll", s), Ie.isPressed && (Ie._vx.reset(), Ie._vy.reset(), a(re ? l : Ge, E[1], xe, !0)), a(qe ? Ge : l, "scroll", ke, fe), a(l, "wheel", Te, fe), a(l, E[0], Ze, fe), a(Ge, E[2], we), a(Ge, E[3], we), a(l, "click", ge, !0), a(l, "click", Ce), a(Ge, "gesturestart", _e), a(Ge, "gestureend", Se), a(l, S + "enter", Ee), a(l, S + "leave", Pe), a(l, S + "move", Me), Ie.isEnabled = Ie.isPressed = Ie.isDragging = !1, le && le(Ie))
        }, Ie.kill = function () {
            Ie.disable();
            var e = C.indexOf(Ie);
            0 <= e && C.splice(e, 1), M === Ie && (M = 0)
        }, C.push(Ie), re && i(l) && (M = Ie), Ie.enable(B)
    }, function (e, n, r) {
        n && t(e.prototype, n), r && t(e, r)
    }(H, [{
        key: "velocityX", get: function () {
            return this._vx.getVelocity()
        }
    }, {
        key: "velocityY", get: function () {
            return this._vy.getVelocity()
        }
    }]), H);

    function H(e) {
        this.init(e)
    }

    function L() {
        return Le = 1
    }

    function N() {
        return Le = 0
    }

    function W(e) {
        return e
    }

    function F(e) {
        return Math.round(1e5 * e) / 1e5 || 0
    }

    function V() {
        return "undefined" != typeof window
    }

    function q() {
        return Ee || V() && (Ee = window.gsap) && Ee.registerPlugin && Ee
    }

    function G(e) {
        return !!~Ye.indexOf(e)
    }

    function U(e) {
        return r(e, "getBoundingClientRect") || (G(e) ? function () {
            return Ut.width = Ce.innerWidth, Ut.height = Ce.innerHeight, Ut
        } : function () {
            return kt(e)
        })
    }

    function j(e, t) {
        var n = t.s, i = t.d2, o = t.d, a = t.a;
        return (n = "scroll" + i) && (a = r(e, n)) ? a() - U(e)()[o] : G(e) ? (Ae[n] || De[n]) - (Ce["inner" + i] || Ae["client" + i] || De["client" + i]) : e[n] - e["offset" + i]
    }

    function K(e, t) {
        for (var n = 0; n < qe.length; n += 3) t && !~t.indexOf(qe[n + 1]) || e(qe[n], qe[n + 1], qe[n + 2])
    }

    function Z(e) {
        return "string" == typeof e
    }

    function $(e) {
        return "function" == typeof e
    }

    function J(e) {
        return "number" == typeof e
    }

    function Q(e) {
        return "object" == typeof e
    }

    function ee(e) {
        return $(e) && e()
    }

    function te(e, t) {
        return function () {
            var n = ee(e), r = ee(t);
            return function () {
                ee(n), ee(r)
            }
        }
    }

    function ne(e, t, n) {
        return e && e.progress(t ? 0 : 1) && n && e.pause()
    }

    function re(e, t) {
        if (e.enabled) {
            var n = t(e);
            n && n.totalTime && (e.callbackAnimation = n)
        }
    }

    function ie(e) {
        return Ce.getComputedStyle(e)
    }

    function oe(e, t) {
        for (var n in t) n in e || (e[n] = t[n]);
        return e
    }

    function ae(e, t) {
        var n = t.d2;
        return e["offset" + n] || e["client" + n] || 0
    }

    function se(e) {
        var t, n = [], r = e.labels, i = e.duration();
        for (t in r) n.push(r[t] / i);
        return n
    }

    function le(e) {
        var t = Ee.utils.snap(e), n = Array.isArray(e) && e.slice(0).sort((function (e, t) {
            return e - t
        }));
        return n ? function (e, r, i) {
            var o;
            if (void 0 === i && (i = .001), !r) return t(e);
            if (0 < r) {
                for (e -= i, o = 0; o < n.length; o++) if (n[o] >= e) return n[o];
                return n[o - 1]
            }
            for (o = n.length, e += i; o--;) if (n[o] <= e) return n[o];
            return n[0]
        } : function (n, r, i) {
            void 0 === i && (i = .001);
            var o = t(n);
            return !r || Math.abs(o - n) < i || o - n < 0 == r < 0 ? o : t(r < 0 ? n - e : n + e)
        }
    }

    function ce(e, t, n, r) {
        return n.split(",").forEach((function (n) {
            return e(t, n, r)
        }))
    }

    function ue(e, t, n, r, i) {
        return e.addEventListener(t, n, {passive: !r, capture: !!i})
    }

    function fe(e, t, n, r) {
        return e.removeEventListener(t, n, !!r)
    }

    function de(e, t, n) {
        return n && n.wheelHandler && e(t, "wheel", n)
    }

    function pe(e, t) {
        if (Z(e)) {
            var n = e.indexOf("="), r = ~n ? (e.charAt(n - 1) + 1) * parseFloat(e.substr(n + 1)) : 0;
            ~n && (e.indexOf("%") > n && (r *= t / 100), e = e.substr(0, n - 1)), e = r + (e in Et ? Et[e] * t : ~e.indexOf("%") ? parseFloat(e) * t / 100 : parseFloat(e) || 0)
        }
        return e
    }

    function he(e, t, n, i, o, a, s, l) {
        var c = o.startColor, u = o.endColor, f = o.fontSize, d = o.indent, p = o.fontWeight,
            h = Oe.createElement("div"), g = G(n) || "fixed" === r(n, "pinType"), v = -1 !== e.indexOf("scroller"),
            m = g ? De : n, y = -1 !== e.indexOf("start"), b = y ? c : u,
            x = "border-color:" + b + ";font-size:" + f + ";color:" + b + ";font-weight:" + p + ";pointer-events:none;white-space:nowrap;font-family:sans-serif,Arial;z-index:1000;padding:4px 8px;border-width:0;border-style:solid;";
        return x += "position:" + ((v || l) && g ? "fixed;" : "absolute;"), !v && !l && g || (x += (i === I ? dt : pt) + ":" + (a + parseFloat(d)) + "px;"), s && (x += "box-sizing:border-box;text-align:left;width:" + s.offsetWidth + "px;"), h._isStart = y, h.setAttribute("class", "gsap-marker-" + e + (t ? " marker-" + t : "")), h.style.cssText = x, h.innerText = t || 0 === t ? e + "-" + t : e, m.children[0] ? m.insertBefore(h, m.children[0]) : m.appendChild(h), h._offset = h["offset" + i.op.d2], Pt(h, 0, i, y), h
    }

    function ge() {
        return 34 < st() - ct && Wt()
    }

    function ve() {
        Ke && Ke.isPressed && !(Ke.startX > De.clientWidth) || (O.cache++, tt = tt || requestAnimationFrame(Wt), ct || Rt("scrollStart"), ct = st())
    }

    function me() {
        Je = Ce.innerWidth, $e = Ce.innerHeight
    }

    function ye() {
        O.cache++, He || je || Oe.fullscreenElement || Oe.webkitFullscreenElement || Ze && Je === Ce.innerWidth && !(Math.abs(Ce.innerHeight - $e) > .25 * Ce.innerHeight) || Re.restart(!0)
    }

    function be(e) {
        var t, n = Ee.ticker.frame, r = [], i = 0;
        if (rt !== n || at) {
            for (It(); i < Yt.length; i += 4) (t = Ce.matchMedia(Yt[i]).matches) !== Yt[i + 3] && ((Yt[i + 3] = t) ? r.push(i) : It(1, Yt[i]) || $(Yt[i + 2]) && Yt[i + 2]());
            for (zt(), i = 0; i < r.length; i++) t = r[i], nt = Yt[t], Yt[t + 2] = Yt[t + 1](e);
            nt = 0, Pe && Ht(0, 1), rt = n, Rt("matchMedia")
        }
    }

    function xe() {
        return fe(Kt, "scrollEnd", xe) || Ht(!0)
    }

    function we() {
        return O.cache++ && O.forEach((function (e) {
            return "function" == typeof e && (e.rec = 0)
        }))
    }

    function _e(e, t, n, r) {
        if (e.parentNode !== t) {
            for (var i, o = Ft.length, a = t.style, s = e.style; o--;) a[i = Ft[o]] = n[i];
            a.position = "absolute" === n.position ? "absolute" : "relative", "inline" === n.display && (a.display = "inline-block"), s[pt] = s[dt] = a.flexBasis = "auto", a.overflow = "visible", a.boxSizing = "border-box", a[ht] = ae(e, z) + St, a[gt] = ae(e, I) + St, a[xt] = s[wt] = s.top = s.left = "0", Gt(r), s[ht] = s.maxWidth = n[ht], s[gt] = s.maxHeight = n[gt], s[xt] = n[xt], e.parentNode.insertBefore(t, e), t.appendChild(e)
        }
    }

    function Se(e) {
        for (var t = Vt.length, n = e.style, r = [], i = 0; i < t; i++) r.push(Vt[i], n[Vt[i]]);
        return r.t = e, r
    }

    function ke(e, t, n, r, i, o, a, s, l, u, f, d, p) {
        $(e) && (e = e(s)), Z(e) && "max" === e.substr(0, 3) && (e = d + ("=" === e.charAt(4) ? pe("0" + e.substr(3), n) : 0));
        var h, g, v, m = p ? p.time() : 0;
        if (p && p.seek(0), J(e)) a && Pt(a, n, r, !0); else {
            $(t) && (t = t(s));
            var y, b, x, w, _ = e.split(" ");
            v = c(t) || De, (y = kt(v) || {}) && (y.left || y.top) || "none" !== ie(v).display || (w = v.style.display, v.style.display = "block", y = kt(v), w ? v.style.display = w : v.style.removeProperty("display")), b = pe(_[0], y[r.d]), x = pe(_[1] || "0", n), e = y[r.p] - l[r.p] - u + b + i - x, a && Pt(a, x, r, n - x < 20 || a._isStart && 20 < x), n -= n - x
        }
        if (o) {
            var S = e + n, k = o._isStart;
            h = "scroll" + r.d2, Pt(o, S, r, k && 20 < S || !k && (f ? Math.max(De[h], Ae[h]) : o.parentNode[h]) <= S + 1), f && (l = kt(a), f && (o.style[r.op.p] = l[r.op.p] - r.op.m - o._offset + St))
        }
        return p && v && (h = kt(v), p.seek(d), g = kt(v), p._caScrollDist = h[r.p] - g[r.p], e = e / p._caScrollDist * d), p && p.seek(m), p ? e : Math.round(e)
    }

    function Te(e, t, n, r) {
        if (e.parentNode !== t) {
            var i, o, a = e.style;
            if (t === De) {
                for (i in e._stOrig = a.cssText, o = ie(e)) +i || jt.test(i) || !o[i] || "string" != typeof a[i] || "0" === i || (a[i] = o[i]);
                a.top = n, a.left = r
            } else a.cssText = e._stOrig;
            Ee.core.getCache(e).uncache = 1, t.appendChild(e)
        }
    }

    function Me(e, t) {
        function n(t, s, l, c, u) {
            var f = n.tween, d = s.onComplete;
            return l = l || o(), u = c && u || 0, c = c || t - l, f && f.kill(), r = Math.round(l), s[a] = t, (s.modifiers = {})[a] = function (e) {
                return (e = F(o())) !== r && e !== i && 2 < Math.abs(e - r) && 2 < Math.abs(e - i) ? (f.kill(), n.tween = 0) : e = l + c * f.ratio + u * f.ratio * f.ratio, i = r, r = F(e)
            }, s.onComplete = function () {
                n.tween = 0, d && d.call(f)
            }, f = n.tween = Ee.to(e, s)
        }

        var r, i, o = u(e, t), a = "_scroll" + t.p2;
        return (e[a] = o).wheelHandler = function () {
            return n.tween && n.tween.kill() && (n.tween = 0)
        }, ue(e, "wheel", o.wheelHandler), n
    }

    B.version = "3.10.4", B.create = function (e) {
        return new B(e)
    }, B.register = g, B.getAll = function () {
        return C.slice()
    }, B.getById = function (e) {
        return C.filter((function (t) {
            return t.vars.id === e
        }))[0]
    }, n() && v.registerPlugin(B);
    var Ee, Pe, Ce, Oe, Ae, De, Ye, Re, Xe, ze, Ie, Be, He, Le, Ne, We, Fe, Ve, qe, Ge, Ue, je, Ke, Ze, $e, Je, Qe, et,
        tt, nt, rt, it, ot, at = 1, st = Date.now, lt = st(), ct = 0, ut = 0, ft = Math.abs, dt = "right",
        pt = "bottom", ht = "width", gt = "height", vt = "Right", mt = "Left", yt = "Top", bt = "Bottom",
        xt = "padding", wt = "margin", _t = "Width", St = "px", kt = function (e, t) {
            var n = t && "matrix(1, 0, 0, 1, 0, 0)" !== ie(e)[Ne] && Ee.to(e, {
                x: 0,
                y: 0,
                xPercent: 0,
                yPercent: 0,
                rotation: 0,
                rotationX: 0,
                rotationY: 0,
                scale: 1,
                skewX: 0,
                skewY: 0
            }).progress(1), r = e.getBoundingClientRect();
            return n && n.progress(0).kill(), r
        }, Tt = {startColor: "green", endColor: "red", indent: 0, fontSize: "16px", fontWeight: "normal"},
        Mt = {toggleActions: "play", anticipatePin: 0}, Et = {top: 0, left: 0, center: .5, bottom: 1, right: 1},
        Pt = function (e, t, n, r) {
            var i = {display: "block"}, o = n[r ? "os2" : "p2"], a = n[r ? "p2" : "os2"];
            e._isFlipped = r, i[n.a + "Percent"] = r ? -100 : 0, i[n.a] = r ? "1px" : 0, i["border" + o + _t] = 1, i["border" + a + _t] = 0, i[n.p] = t + "px", Ee.set(e, i)
        }, Ct = [], Ot = {}, At = {}, Dt = [], Yt = [], Rt = function (e) {
            return At[e] && At[e].map((function (e) {
                return e()
            })) || Dt
        }, Xt = [], zt = function (e) {
            for (var t = 0; t < Xt.length; t += 5) e && Xt[t + 4] !== e || (Xt[t].style.cssText = Xt[t + 1], Xt[t].getBBox && Xt[t].setAttribute("transform", Xt[t + 2] || ""), Xt[t + 3].uncache = 1)
        }, It = function (e, t) {
            var n;
            for (We = 0; We < Ct.length; We++) n = Ct[We], t && n.media !== t || (e ? n.kill(1) : n.revert());
            t && zt(t), t || Rt("revert")
        }, Bt = 0, Ht = function (e, t) {
            if (!ct || e) {
                it = !0;
                var n = Rt("refreshInit");
                Ge && Kt.sort(), t || It(), Ct.slice(0).forEach((function (e) {
                    return e.refresh()
                })), Ct.forEach((function (e) {
                    return "max" === e.vars.end && e.setPositions(e.start, j(e.scroller, e._dir))
                })), n.forEach((function (e) {
                    return e && e.render && e.render(-1)
                })), we(), Re.pause(), Bt++, it = !1, Rt("refresh")
            } else ue(Kt, "scrollEnd", xe)
        }, Lt = 0, Nt = 1, Wt = function () {
            if (!it) {
                Kt.isUpdating = !0, ot && ot.update(0);
                var e = Ct.length, t = st(), n = 50 <= t - lt, r = e && Ct[0].scroll();
                if (Nt = r < Lt ? -1 : 1, Lt = r, n && (ct && !Le && 200 < t - ct && (ct = 0, Rt("scrollEnd")), Ie = lt, lt = t), Nt < 0) {
                    for (We = e; 0 < We--;) Ct[We] && Ct[We].update(0, n);
                    Nt = 1
                } else for (We = 0; We < e; We++) Ct[We] && Ct[We].update(0, n);
                Kt.isUpdating = !1
            }
            tt = 0
        },
        Ft = ["left", "top", pt, dt, wt + bt, wt + vt, wt + yt, wt + mt, "display", "flexShrink", "float", "zIndex", "gridColumnStart", "gridColumnEnd", "gridRowStart", "gridRowEnd", "gridArea", "justifySelf", "alignSelf", "placeSelf", "order"],
        Vt = Ft.concat([ht, gt, "boxSizing", "max" + _t, "maxHeight", "position", wt, xt, xt + yt, xt + vt, xt + bt, xt + mt]),
        qt = /([A-Z])/g, Gt = function (e) {
            if (e) {
                var t, n, r = e.t.style, i = e.length, o = 0;
                for ((e.t._gsap || Ee.core.getCache(e.t)).uncache = 1; o < i; o += 2) n = e[o + 1], t = e[o], n ? r[t] = n : r[t] && r.removeProperty(t.replace(qt, "-$1").toLowerCase())
            }
        }, Ut = {left: 0, top: 0}, jt = /(webkit|moz|length|cssText|inset)/i, Kt = (Zt.prototype.init = function (e, t) {
            if (this.progress = this.start = 0, this.vars && this.kill(!0, !0), ut) {
                var n, i, o, a, s, l, f, d, p, h, g, v, m, y, b, x, w, _, S, k, T, M, E, P, C, O, D, Y, R, X, B, H, L, N, V,
                    q, K, ee, te = (e = oe(Z(e) || J(e) || e.nodeType ? {trigger: e} : e, Mt)).onUpdate, ce = e.toggleClass,
                    de = e.id, ge = e.onToggle, me = e.onRefresh, be = e.scrub, we = e.trigger, Pe = e.pin,
                    Ye = e.pinSpacing, Re = e.invalidateOnRefresh, Be = e.anticipatePin, Ne = e.onScrubComplete,
                    Fe = e.onSnapComplete, Ve = e.once, qe = e.snap, je = e.pinReparent, Ke = e.pinSpacer,
                    Ze = e.containerAnimation, $e = e.fastScrollEnd, Je = e.preventOverlaps,
                    Qe = e.horizontal || e.containerAnimation && !1 !== e.horizontal ? z : I, tt = !be && 0 !== be,
                    rt = c(e.scroller || Ce), lt = Ee.core.getCache(rt), dt = G(rt),
                    pt = "fixed" === ("pinType" in e ? e.pinType : r(rt, "pinType") || dt && "fixed"),
                    Et = [e.onEnter, e.onLeave, e.onEnterBack, e.onLeaveBack], Pt = tt && e.toggleActions.split(" "),
                    At = "markers" in e ? e.markers : Mt.markers,
                    Dt = dt ? 0 : parseFloat(ie(rt)["border" + Qe.p2 + _t]) || 0, Yt = this,
                    Rt = e.onRefreshInit && function () {
                        return e.onRefreshInit(Yt)
                    }, Xt = function (e, t, n) {
                        var i = n.d, o = n.d2, a = n.a;
                        return (a = r(e, "getBoundingClientRect")) ? function () {
                            return a()[i]
                        } : function () {
                            return (t ? Ce["inner" + o] : e["client" + o]) || 0
                        }
                    }(rt, dt, Qe), zt = function (e, t) {
                        return !t || ~A.indexOf(e) ? U(e) : function () {
                            return Ut
                        }
                    }(rt, dt), It = 0, Bt = 0, Ht = u(rt, Qe);
                if (Yt.media = nt, Yt._dir = Qe, Be *= 45, Yt.scroller = rt, Yt.scroll = Ze ? Ze.time.bind(Ze) : Ht, a = Ht(), Yt.vars = e, t = t || e.animation, "refreshPriority" in e && (Ge = 1, -9999 === e.refreshPriority && (ot = Yt)), lt.tweenScroll = lt.tweenScroll || {
                    top: Me(rt, I),
                    left: Me(rt, z)
                }, Yt.tweenTo = n = lt.tweenScroll[Qe.p], Yt.scrubDuration = function (e) {
                    (B = J(e) && e) ? X ? X.duration(e) : X = Ee.to(t, {
                        ease: "expo",
                        totalProgress: "+=0.001",
                        duration: B,
                        paused: !0,
                        onComplete: function () {
                            return Ne && Ne(Yt)
                        }
                    }) : (X && X.progress(1).kill(), X = 0)
                }, t && (t.vars.lazy = !1, t._initted || !1 !== t.vars.immediateRender && !1 !== e.immediateRender && t.render(0, !0, !0), Yt.animation = t.pause(), (t.scrollTrigger = Yt).scrubDuration(be), Y = 0, de = de || t.vars.id), Ct.push(Yt), qe && (Q(qe) && !qe.push || (qe = {snapTo: qe}), "scrollBehavior" in De.style && Ee.set(dt ? [De, Ae] : rt, {scrollBehavior: "auto"}), o = $(qe.snapTo) ? qe.snapTo : "labels" === qe.snapTo ? function (e) {
                    return function (t) {
                        return Ee.utils.snap(se(e), t)
                    }
                }(t) : "labelsDirectional" === qe.snapTo ? function (e) {
                    return function (t, n) {
                        return le(se(e))(t, n.direction)
                    }
                }(t) : !1 !== qe.directional ? function (e, t) {
                    return le(qe.snapTo)(e, st() - Bt < 500 ? 0 : t.direction)
                } : Ee.utils.snap(qe.snapTo), H = Q(H = qe.duration || {
                    min: .1,
                    max: 2
                }) ? ze(H.min, H.max) : ze(H, H), L = Ee.delayedCall(qe.delay || B / 2 || .1, (function () {
                    var e = Ht(), r = st() - Bt < 500, i = n.tween;
                    if (!(r || Math.abs(Yt.getVelocity()) < 10) || i || Le || It === e) Yt.isActive && It !== e && L.restart(!0); else {
                        var a = (e - l) / m, s = t && !tt ? t.totalProgress() : a,
                            c = r ? 0 : (s - R) / (st() - Ie) * 1e3 || 0,
                            u = Ee.utils.clamp(-a, 1 - a, ft(c / 2) * c / .185), d = a + (!1 === qe.inertia ? 0 : u),
                            p = ze(0, 1, o(d, Yt)), h = Math.round(l + p * m), g = qe.onStart, v = qe.onInterrupt,
                            y = qe.onComplete;
                        if (e <= f && l <= e && h !== e) {
                            if (i && !i._initted && i.data <= ft(h - e)) return;
                            !1 === qe.inertia && (u = p - a), n(h, {
                                duration: H(ft(.185 * Math.max(ft(d - s), ft(p - s)) / c / .05 || 0)),
                                ease: qe.ease || "power3",
                                data: ft(h - e),
                                onInterrupt: function () {
                                    return L.restart(!0) && v && v(Yt)
                                },
                                onComplete: function () {
                                    Yt.update(), It = Ht(), Y = R = t && !tt ? t.totalProgress() : Yt.progress, Fe && Fe(Yt), y && y(Yt)
                                }
                            }, e, u * m, h - e - u * m), g && g(Yt, n.tween)
                        }
                    }
                })).pause()), de && (Ot[de] = Yt), ee = (ee = (we = Yt.trigger = c(we || Pe)) && we._gsap && we._gsap.stRevert) && ee(Yt), Pe = !0 === Pe ? we : c(Pe), Z(ce) && (ce = {
                    targets: we,
                    className: ce
                }), Pe && (!1 === Ye || Ye === wt || (Ye = !(!Ye && "flex" === ie(Pe.parentNode).display) && xt), Yt.pin = Pe, !1 !== e.force3D && Ee.set(Pe, {force3D: !0}), (i = Ee.core.getCache(Pe)).spacer ? y = i.pinState : (Ke && ((Ke = c(Ke)) && !Ke.nodeType && (Ke = Ke.current || Ke.nativeElement), i.spacerIsNative = !!Ke, Ke && (i.spacerState = Se(Ke))), i.spacer = w = Ke || Oe.createElement("div"), w.classList.add("pin-spacer"), de && w.classList.add("pin-spacer-" + de), i.pinState = y = Se(Pe)), Yt.spacer = w = i.spacer, D = ie(Pe), E = D[Ye + Qe.os2], S = Ee.getProperty(Pe), k = Ee.quickSetter(Pe, Qe.a, St), _e(Pe, w, D), x = Se(Pe)), At) {
                    v = Q(At) ? oe(At, Tt) : Tt, h = he("scroller-start", de, rt, Qe, v, 0), g = he("scroller-end", de, rt, Qe, v, 0, h), _ = h["offset" + Qe.op.d2];
                    var Lt = c(r(rt, "content") || rt);
                    d = this.markerStart = he("start", de, Lt, Qe, v, _, 0, Ze), p = this.markerEnd = he("end", de, Lt, Qe, v, _, 0, Ze), Ze && (K = Ee.quickSetter([d, p], Qe.a, St)), pt || A.length && !0 === r(rt, "fixedMarkers") || (function (e) {
                        var t = ie(e).position;
                        e.style.position = "absolute" === t || "fixed" === t ? t : "relative"
                    }(dt ? De : rt), Ee.set([h, g], {force3D: !0}), C = Ee.quickSetter(h, Qe.a, St), O = Ee.quickSetter(g, Qe.a, St))
                }
                if (Ze) {
                    var Wt = Ze.vars.onUpdate, Ft = Ze.vars.onUpdateParams;
                    Ze.eventCallback("onUpdate", (function () {
                        Yt.update(0, 0, 1), Wt && Wt.apply(Ft || [])
                    }))
                }
                Yt.previous = function () {
                    return Ct[Ct.indexOf(Yt) - 1]
                }, Yt.next = function () {
                    return Ct[Ct.indexOf(Yt) + 1]
                }, Yt.revert = function (e) {
                    var n = !1 !== e || !Yt.enabled, r = He;
                    n !== Yt.isReverted && (n && (!Yt.scroll.rec && He && it && (Yt.scroll.rec = Ht()), V = Math.max(Ht(), Yt.scroll.rec || 0), N = Yt.progress, q = t && t.progress()), d && [d, p, h, g].forEach((function (e) {
                        return e.style.display = n ? "none" : "block"
                    })), n && (He = 1), Yt.update(n), He = r, Pe && (n ? function (e, t, n) {
                        Gt(n);
                        var r = e._gsap;
                        if (r.spacerIsNative) Gt(r.spacerState); else if (e.parentNode === t) {
                            var i = t.parentNode;
                            i && (i.insertBefore(e, t), i.removeChild(t))
                        }
                    }(Pe, w, y) : je && Yt.isActive || _e(Pe, w, ie(Pe), P)), Yt.isReverted = n)
                }, Yt.refresh = function (r, i) {
                    if (!He && Yt.enabled || i) if (Pe && r && ct) ue(Zt, "scrollEnd", xe); else {
                        !it && Rt && Rt(Yt), He = 1, Bt = st(), n.tween && (n.tween.kill(), n.tween = 0), X && X.pause(), Re && t && t.time(-.01, !0).invalidate(), Yt.isReverted || Yt.revert();
                        for (var o, v, _, k, E, C, O, A, D, Y, R = Xt(), B = zt(), H = Ze ? Ze.duration() : j(rt, Qe), W = 0, F = 0, G = e.end, U = e.endTrigger || we, K = e.start || (0 !== e.start && we ? Pe ? "0 0" : "0 100%" : 0), Q = Yt.pinnedContainer = e.pinnedContainer && c(e.pinnedContainer), ee = we && Math.max(0, Ct.indexOf(Yt)) || 0, te = ee; te--;) (C = Ct[te]).end || C.refresh(0, 1) || (He = 1), !(O = C.pin) || O !== we && O !== Pe || C.isReverted || ((Y = Y || []).unshift(C), C.revert()), C !== Ct[te] && (ee--, te--);
                        for ($(K) && (K = K(Yt)), l = ke(K, we, R, Qe, Ht(), d, h, Yt, B, Dt, pt, H, Ze) || (Pe ? -.001 : 0), $(G) && (G = G(Yt)), Z(G) && !G.indexOf("+=") && (~G.indexOf(" ") ? G = (Z(K) ? K.split(" ")[0] : "") + G : (W = pe(G.substr(2), R), G = Z(K) ? K : l + W, U = we)), f = Math.max(l, ke(G || (U ? "100% 0" : H), U, R, Qe, Ht() + W, p, g, Yt, B, Dt, pt, H, Ze)) || -.001, m = f - l || (l -= .01) && .001, W = 0, te = ee; te--;) (O = (C = Ct[te]).pin) && C.start - C._pinPush < l && !Ze && 0 < C.end && (o = C.end - C.start, O !== we && O !== Q || J(K) || (W += o * (1 - C.progress)), O === Pe && (F += o));
                        if (l += W, f += W, Yt._pinPush = F, d && W && ((o = {})[Qe.a] = "+=" + W, Q && (o[Qe.p] = "-=" + Ht()), Ee.set([d, p], o)), Pe) o = ie(Pe), k = Qe === I, _ = Ht(), T = parseFloat(S(Qe.a)) + F, !H && 1 < f && ((dt ? De : rt).style["overflow-" + Qe.a] = "scroll"), _e(Pe, w, o), x = Se(Pe), v = kt(Pe, !0), A = pt && u(rt, k ? z : I)(), Ye && ((P = [Ye + Qe.os2, m + F + St]).t = w, (te = Ye === xt ? ae(Pe, Qe) + m + F : 0) && P.push(Qe.d, te + St), Gt(P), pt && Ht(V)), pt && ((E = {
                            top: v.top + (k ? _ - l : A) + St,
                            left: v.left + (k ? A : _ - l) + St,
                            boxSizing: "border-box",
                            position: "fixed"
                        })[ht] = E.maxWidth = Math.ceil(v.width) + St, E[gt] = E.maxHeight = Math.ceil(v.height) + St, E[wt] = E[wt + yt] = E[wt + vt] = E[wt + bt] = E[wt + mt] = "0", E[xt] = o[xt], E[xt + yt] = o[xt + yt], E[xt + vt] = o[xt + vt], E[xt + bt] = o[xt + bt], E[xt + mt] = o[xt + mt], b = function (e, t, n) {
                            for (var r, i = [], o = e.length, a = n ? 8 : 0; a < o; a += 2) r = e[a], i.push(r, r in t ? t[r] : e[a + 1]);
                            return i.t = e.t, i
                        }(y, E, je)), t ? (D = t._initted, Ue(1), t.render(t.duration(), !0, !0), M = S(Qe.a) - T + m + F, m !== M && pt && b.splice(b.length - 2, 2), t.render(0, !0, !0), D || t.invalidate(), Ue(0)) : M = m; else if (we && Ht() && !Ze) for (v = we.parentNode; v && v !== De;) v._pinOffset && (l -= v._pinOffset, f -= v._pinOffset), v = v.parentNode;
                        Y && Y.forEach((function (e) {
                            return e.revert(!1)
                        })), Yt.start = l, Yt.end = f, a = s = Ht(), Ze || (a < V && Ht(V), Yt.scroll.rec = 0), Yt.revert(!1), L && (It = -1, Yt.isActive && Ht(l + m * N), L.restart(!0)), He = 0, t && tt && (t._initted || q) && t.progress() !== q && t.progress(q, !0).render(t.time(), !0, !0), N === Yt.progress && !Ze || (t && !tt && t.totalProgress(N, !0), Yt.progress = N, Yt.update(0, 0, 1)), Pe && Ye && (w._pinOffset = Math.round(Yt.progress * M)), me && me(Yt)
                    }
                }, Yt.getVelocity = function () {
                    return (Ht() - s) / (st() - Ie) * 1e3 || 0
                }, Yt.endAnimation = function () {
                    ne(Yt.callbackAnimation), t && (X ? X.progress(1) : t.paused() ? tt || ne(t, Yt.direction < 0, 1) : ne(t, t.reversed()))
                }, Yt.labelToScroll = function (e) {
                    return t && t.labels && (l || Yt.refresh() || l) + t.labels[e] / t.duration() * m || 0
                }, Yt.getTrailing = function (e) {
                    var t = Ct.indexOf(Yt), n = 0 < Yt.direction ? Ct.slice(0, t).reverse() : Ct.slice(t + 1);
                    return (Z(e) ? n.filter((function (t) {
                        return t.vars.preventOverlaps === e
                    })) : n).filter((function (e) {
                        return 0 < Yt.direction ? e.end <= l : e.start >= f
                    }))
                }, Yt.update = function (e, r, i) {
                    if (!Ze || i || e) {
                        var o, c, u, d, p, g, v, y = Yt.scroll(), _ = e ? 0 : (y - l) / m,
                            S = _ < 0 ? 0 : 1 < _ ? 1 : _ || 0, P = Yt.progress;
                        if (r && (s = a, a = Ze ? Ht() : y, qe && (R = Y, Y = t && !tt ? t.totalProgress() : S)), Be && !S && Pe && !He && !at && ct && l < y + (y - s) / (st() - Ie) * Be && (S = 1e-4), S !== P && Yt.enabled) {
                            if (d = (p = (o = Yt.isActive = !!S && S < 1) != (!!P && P < 1)) || !!S != !!P, Yt.direction = P < S ? 1 : -1, Yt.progress = S, d && !He && (c = S && !P ? 0 : 1 === S ? 1 : 1 === P ? 2 : 3, tt && (u = !p && "none" !== Pt[c + 1] && Pt[c + 1] || Pt[c], v = t && ("complete" === u || "reset" === u || u in t))), Je && (p || v) && (v || be || !t) && ($(Je) ? Je(Yt) : Yt.getTrailing(Je).forEach((function (e) {
                                return e.endAnimation()
                            }))), tt || (!X || He || at ? t && t.totalProgress(S, !!He) : ((Ze || ot && ot !== Yt) && X.render(X._dp._time - X._start), X.resetTo ? X.resetTo("totalProgress", S, t._tTime / t._tDur) : (X.vars.totalProgress = S, X.invalidate().restart()))), Pe) if (e && Ye && (w.style[Ye + Qe.os2] = E), pt) {
                                if (d) {
                                    if (g = !e && P < S && y < f + 1 && y + 1 >= j(rt, Qe), je) if (e || !o && !g) Te(Pe, w); else {
                                        var A = kt(Pe, !0), D = y - l;
                                        Te(Pe, De, A.top + (Qe === I ? D : 0) + St, A.left + (Qe === I ? 0 : D) + St)
                                    }
                                    Gt(o || g ? b : x), M !== m && S < 1 && o || k(T + (1 !== S || g ? 0 : M))
                                }
                            } else k(F(T + M * S));
                            !qe || n.tween || He || at || L.restart(!0), ce && (p || Ve && S && (S < 1 || !et)) && Xe(ce.targets).forEach((function (e) {
                                return e.classList[o || Ve ? "add" : "remove"](ce.className)
                            })), !te || tt || e || te(Yt), d && !He ? (tt && (v && ("complete" === u ? t.pause().totalProgress(1) : "reset" === u ? t.restart(!0).pause() : "restart" === u ? t.restart(!0) : t[u]()), te && te(Yt)), !p && et || (ge && p && re(Yt, ge), Et[c] && re(Yt, Et[c]), Ve && (1 === S ? Yt.kill(!1, 1) : Et[c] = 0), p || Et[c = 1 === S ? 1 : 3] && re(Yt, Et[c])), $e && !o && Math.abs(Yt.getVelocity()) > (J($e) ? $e : 2500) && (ne(Yt.callbackAnimation), X ? X.progress(1) : ne(t, !S, 1))) : tt && te && !He && te(Yt)
                        }
                        if (O) {
                            var z = Ze ? y / Ze.duration() * (Ze._caScrollDist || 0) : y;
                            C(z + (h._isFlipped ? 1 : 0)), O(z)
                        }
                        K && K(-y / Ze.duration() * (Ze._caScrollDist || 0))
                    }
                }, Yt.enable = function (e, t) {
                    Yt.enabled || (Yt.enabled = !0, ue(rt, "resize", ye), ue(dt ? Oe : rt, "scroll", ve), Rt && ue(Zt, "refreshInit", Rt), !1 !== e && (Yt.progress = N = 0, a = s = It = Ht()), !1 !== t && Yt.refresh())
                }, Yt.getTween = function (e) {
                    return e && n ? n.tween : X
                }, Yt.setPositions = function (e, t) {
                    Pe && (T += e - l, M += t - e - m), Yt.start = l = e, Yt.end = f = t, m = t - e, Yt.update()
                }, Yt.disable = function (e, t) {
                    if (Yt.enabled && (!1 !== e && Yt.revert(), Yt.enabled = Yt.isActive = !1, t || X && X.pause(), V = 0, i && (i.uncache = 1), Rt && fe(Zt, "refreshInit", Rt), L && (L.pause(), n.tween && n.tween.kill() && (n.tween = 0)), !dt)) {
                        for (var r = Ct.length; r--;) if (Ct[r].scroller === rt && Ct[r] !== Yt) return;
                        fe(rt, "resize", ye), fe(rt, "scroll", ve)
                    }
                }, Yt.kill = function (n, r) {
                    Yt.disable(n, r), X && !r && X.kill(), de && delete Ot[de];
                    var o = Ct.indexOf(Yt);
                    0 <= o && Ct.splice(o, 1), o === We && 0 < Nt && We--, o = 0, Ct.forEach((function (e) {
                        return e.scroller === Yt.scroller && (o = 1)
                    })), o || (Yt.scroll.rec = 0), t && (t.scrollTrigger = null, n && t.render(-1), r || t.kill()), d && [d, p, h, g].forEach((function (e) {
                        return e.parentNode && e.parentNode.removeChild(e)
                    })), ot === Yt && (ot = 0), Pe && (i && (i.uncache = 1), o = 0, Ct.forEach((function (e) {
                        return e.pin === Pe && o++
                    })), o || (i.spacer = 0)), e.onKill && e.onKill(Yt)
                }, Yt.enable(!1, !1), ee && ee(Yt), t && t.add && !m ? Ee.delayedCall(.01, (function () {
                    return l || f || Yt.refresh()
                })) && (m = .01) && (l = f = 0) : Yt.refresh()
            } else this.update = this.refresh = this.kill = W
        }, Zt.register = function (e) {
            return Pe || (Ee = e || q(), V() && window.document && Zt.enable(), Pe = ut), Pe
        }, Zt.defaults = function (e) {
            if (e) for (var t in e) Mt[t] = e[t];
            return Mt
        }, Zt.disable = function (e, t) {
            ut = 0, Ct.forEach((function (n) {
                return n[t ? "kill" : "disable"](e)
            })), fe(Ce, "wheel", ve), fe(Oe, "scroll", ve), clearInterval(Be), fe(Oe, "touchcancel", W), fe(De, "touchstart", W), ce(fe, Oe, "pointerdown,touchstart,mousedown", L), ce(fe, Oe, "pointerup,touchend,mouseup", N), Re.kill(), K(fe);
            for (var n = 0; n < O.length; n += 3) de(fe, O[n], O[n + 1]), de(fe, O[n], O[n + 2])
        }, Zt.enable = function () {
            if (Ce = window, Oe = document, Ae = Oe.documentElement, De = Oe.body, Ee && (Xe = Ee.utils.toArray, ze = Ee.utils.clamp, Ue = Ee.core.suppressOverwrites || W, Ee.core.globals("ScrollTrigger", Zt), De)) {
                ut = 1, B.register(Ee), Zt.isTouch = B.isTouch, Qe = B.isTouch && /(iPad|iPhone|iPod|Mac)/g.test(navigator.userAgent), ue(Ce, "wheel", ve), Ye = [Ce, Oe, Ae, De], Zt.matchMedia({
                    "(orientation: portrait)": function () {
                        return me(), me
                    }
                }), ue(Oe, "scroll", ve);
                var e, t, n = De.style, r = n.borderTopStyle;
                for (n.borderTopStyle = "solid", e = kt(De), I.m = Math.round(e.top + I.sc()) || 0, z.m = Math.round(e.left + z.sc()) || 0, r ? n.borderTopStyle = r : n.removeProperty("border-top-style"), Be = setInterval(ge, 250), Ee.delayedCall(.5, (function () {
                    return at = 0
                })), ue(Oe, "touchcancel", W), ue(De, "touchstart", W), ce(ue, Oe, "pointerdown,touchstart,mousedown", L), ce(ue, Oe, "pointerup,touchend,mouseup", N), Ne = Ee.utils.checkPrefix("transform"), Vt.push(Ne), Pe = st(), Re = Ee.delayedCall(.2, Ht).pause(), qe = [Oe, "visibilitychange", function () {
                    var e = Ce.innerWidth, t = Ce.innerHeight;
                    Oe.hidden ? (Fe = e, Ve = t) : Fe === e && Ve === t || ye()
                }, Oe, "DOMContentLoaded", Ht, Ce, "load", Ht, Ce, "resize", ye], K(ue), Ct.forEach((function (e) {
                    return e.enable(0, 1)
                })), t = 0; t < O.length; t += 3) de(fe, O[t], O[t + 1]), de(fe, O[t], O[t + 2])
            }
        }, Zt.config = function (e) {
            "limitCallbacks" in e && (et = !!e.limitCallbacks);
            var t = e.syncInterval;
            t && clearInterval(Be) || (Be = t) && setInterval(ge, t), "ignoreMobileResize" in e && (Ze = 1 === Zt.isTouch && e.ignoreMobileResize), "autoRefreshEvents" in e && (K(fe) || K(ue, e.autoRefreshEvents || "none"), je = -1 === (e.autoRefreshEvents + "").indexOf("resize"))
        }, Zt.scrollerProxy = function (e, t) {
            var n = c(e), r = O.indexOf(n), i = G(n);
            ~r && O.splice(r, i ? 6 : 2), t && (i ? A.unshift(Ce, t, De, t, Ae, t) : A.unshift(n, t))
        }, Zt.matchMedia = function (e) {
            var t, n, r, i, o;
            for (n in e) r = Yt.indexOf(n), i = e[n], "all" === (nt = n) ? i() : (t = Ce.matchMedia(n)) && (t.matches && (o = i()), ~r ? (Yt[r + 1] = te(Yt[r + 1], i), Yt[r + 2] = te(Yt[r + 2], o)) : (r = Yt.length, Yt.push(n, i, o), t.addListener ? t.addListener(be) : t.addEventListener("change", be)), Yt[r + 3] = t.matches), nt = 0;
            return Yt
        }, Zt.clearMatchMedia = function (e) {
            e || (Yt.length = 0), 0 <= (e = Yt.indexOf(e)) && Yt.splice(e, 4)
        }, Zt.isInViewport = function (e, t, n) {
            var r = (Z(e) ? c(e) : e).getBoundingClientRect(), i = r[n ? ht : gt] * t || 0;
            return n ? 0 < r.right - i && r.left + i < Ce.innerWidth : 0 < r.bottom - i && r.top + i < Ce.innerHeight
        }, Zt.positionInViewport = function (e, t, n) {
            Z(e) && (e = c(e));
            var r = e.getBoundingClientRect(), i = r[n ? ht : gt],
                o = null == t ? i / 2 : t in Et ? Et[t] * i : ~t.indexOf("%") ? parseFloat(t) * i / 100 : parseFloat(t) || 0;
            return n ? (r.left + o) / Ce.innerWidth : (r.top + o) / Ce.innerHeight
        }, Zt);

    function Zt(e, t) {
        Pe || Zt.register(Ee) || console.warn("Please gsap.registerPlugin(ScrollTrigger)"), this.init(e, t)
    }

    function $t(e, t, n, r) {
        return r < t ? e(r) : t < 0 && e(0), r < n ? (r - t) / (n - t) : n < 0 ? t / (t - n) : 1
    }

    function Jt(e, t) {
        !0 === t ? e.style.removeProperty("touch-action") : e.style.touchAction = !0 === t ? "auto" : t ? "pan-" + t + (B.isTouch ? " pinch-zoom" : "") : "none", e === Ae && Jt(De, t)
    }

    function Qt(e) {
        var t, n = e.event, r = e.target, i = e.axis, o = (n.changedTouches ? n.changedTouches[0] : n).target,
            a = o._gsap || Ee.core.getCache(o), s = st();
        if (!a._isScrollT || 2e3 < s - a._isScrollT) {
            for (; o && o.scrollHeight <= o.clientHeight;) o = o.parentNode;
            a._isScroll = o && !G(o) && o !== r && (nn[(t = ie(o)).overflowY] || nn[t.overflowX]), a._isScrollT = s
        }
        !a._isScroll && "x" !== i || (n._gsapAllow = !0)
    }

    function en(e, t, n, r) {
        return B.create({
            target: e,
            capture: !0,
            debounce: !1,
            lockAxis: !0,
            type: t,
            onWheel: r = r && Qt,
            onPress: r,
            onDrag: r,
            onScroll: r,
            onEnable: function () {
                return n && ue(Oe, B.eventTypes[0], on, !1, !0)
            },
            onDisable: function () {
                return fe(Oe, B.eventTypes[0], on, !0)
            }
        })
    }

    Kt.version = "3.10.4", Kt.saveStyles = function (e) {
        return e ? Xe(e).forEach((function (e) {
            if (e && e.style) {
                var t = Xt.indexOf(e);
                0 <= t && Xt.splice(t, 5), Xt.push(e, e.style.cssText, e.getBBox && e.getAttribute("transform"), Ee.core.getCache(e), nt)
            }
        })) : Xt
    }, Kt.revert = function (e, t) {
        return It(!e, t)
    }, Kt.create = function (e, t) {
        return new Kt(e, t)
    }, Kt.refresh = function (e) {
        return e ? ye() : (Pe || Kt.register()) && Ht(!0)
    }, Kt.update = Wt, Kt.clearScrollMemory = we, Kt.maxScroll = function (e, t) {
        return j(e, t ? z : I)
    }, Kt.getScrollFunc = function (e, t) {
        return u(c(e), t ? z : I)
    }, Kt.getById = function (e) {
        return Ot[e]
    }, Kt.getAll = function () {
        return Ct.filter((function (e) {
            return "ScrollSmoother" !== e.vars.id
        }))
    }, Kt.isScrolling = function () {
        return !!ct
    }, Kt.snapDirectional = le, Kt.addEventListener = function (e, t) {
        var n = At[e] || (At[e] = []);
        ~n.indexOf(t) || n.push(t)
    }, Kt.removeEventListener = function (e, t) {
        var n = At[e], r = n && n.indexOf(t);
        0 <= r && n.splice(r, 1)
    }, Kt.batch = function (e, t) {
        function n(e, t) {
            var n = [], r = [], i = Ee.delayedCall(a, (function () {
                t(n, r), n = [], r = []
            })).pause();
            return function (e) {
                n.length || i.restart(!0), n.push(e.trigger), r.push(e), s <= n.length && i.progress(1)
            }
        }

        var r, i = [], o = {}, a = t.interval || .016, s = t.batchMax || 1e9;
        for (r in t) o[r] = "on" === r.substr(0, 2) && $(t[r]) && "onRefreshInit" !== r ? n(0, t[r]) : t[r];
        return $(s) && (s = s(), ue(Kt, "refresh", (function () {
            return s = t.batchMax()
        }))), Xe(e).forEach((function (e) {
            var t = {};
            for (r in o) t[r] = o[r];
            t.trigger = e, i.push(Kt.create(t))
        })), i
    };
    var tn, nn = {auto: 1, scroll: 1}, rn = /(input|label|select|textarea)/i, on = function (e) {
        var t = rn.test(e.target.tagName);
        (t || tn) && (e._gsapAllow = !0, tn = t)
    };
    Kt.sort = function (e) {
        return Ct.sort(e || function (e, t) {
            return -1e6 * (e.vars.refreshPriority || 0) + e.start - (t.start + -1e6 * (t.vars.refreshPriority || 0))
        })
    }, Kt.observe = function (e) {
        return new B(e)
    }, Kt.normalizeScroll = function (e) {
        if (void 0 === e) return Ke;
        if (!0 === e && Ke) return Ke.enable();
        if (!1 === e) return Ke && Ke.kill();
        var t = e instanceof B ? e : function (e) {
            function t() {
                return a = !1
            }

            function n() {
                o = j(m, I), P = ze(Qe ? 1 : 0, o), h && (E = ze(0, j(m, z))), s = Bt
            }

            function r() {
                n(), l.isActive() && l.vars.scrollY > o && (x() > o ? l.progress(1) && x(o) : l.resetTo("scrollY", o))
            }

            Q(e) || (e = {}), e.preventDefault = e.isNormalizer = e.allowClicks = !0, e.type || (e.type = "wheel,touch"), e.debounce = !!e.debounce, e.id = e.id || "normalizer";
            var i, o, a, s, l, f, d, p, h = e.normalizeScrollX, g = e.momentum, v = e.allowNestedScroll,
                m = c(e.target) || Ae, y = Ee.core.globals().ScrollSmoother,
                b = Qe && (e.content && c(e.content) || y && y.get() && y.get().content()), x = u(m, I), w = u(m, z),
                _ = 1,
                S = (B.isTouch && Ce.visualViewport ? Ce.visualViewport.scale * Ce.visualViewport.width : Ce.outerWidth) / Ce.innerWidth,
                k = 0, T = $(g) ? function () {
                    return g(i)
                } : function () {
                    return g || 2.8
                }, M = en(m, e.type, !0, v), E = W, P = W;
            return e.ignoreCheck = function (e) {
                return Qe && "touchmove" === e.type && function () {
                    if (a) {
                        requestAnimationFrame(t);
                        var e = F(i.deltaY / 2), n = P(x.v - e);
                        return b && n !== x.v + x.offset && (x.offset = n - x.v, b.style.transform = "translateY(" + -x.offset + "px)", b._gsap && (b._gsap.y = -x.offset + "px"), x.cacheID = O.cache, Wt()), !0
                    }
                    b && (b.style.transform = "translateY(0px)", x.offset = x.cacheID = 0, b._gsap && (b._gsap.y = "0px")), a = !0
                }() || 1.05 < _ && "touchstart" !== e.type || i.isGesturing || e.touches && 1 < e.touches.length
            }, e.onPress = function () {
                var e = _;
                _ = F((Ce.visualViewport && Ce.visualViewport.scale || 1) / S), l.pause(), e !== _ && Jt(m, 1.01 < _ || !h && "x"), a = !1, f = w(), d = x(), n(), s = Bt
            }, e.onRelease = e.onGestureStart = function (e, t) {
                if (b && (b.style.transform = "translateY(0px)", x.offset = x.cacheID = 0, b._gsap && (b._gsap.y = "0px")), t) {
                    O.cache++;
                    var n, i, a = T();
                    h && (i = (n = w()) + .05 * a * -e.velocityX / .227, a *= $t(w, n, i, j(m, z)), l.vars.scrollX = E(i)), i = (n = x()) + .05 * a * -e.velocityY / .227, a *= $t(x, n, i, j(m, I)), l.vars.scrollY = P(i), l.invalidate().duration(a).play(.01), (Qe && l.vars.scrollY >= o || o - 1 <= n) && Ee.to({}, {
                        onUpdate: r,
                        duration: a
                    })
                } else p.restart(!0)
            }, e.onWheel = function () {
                l._ts && l.pause(), 1e3 < st() - k && (s = 0, k = st())
            }, e.onChange = function (e, t, r, i, o) {
                Bt !== s && n(), t && h && w(E(i[2] === t ? f + (e.startX - e.x) : w() + t - i[1])), r && x(P(o[2] === r ? d + (e.startY - e.y) : x() + r - o[1])), Wt()
            }, e.onEnable = function () {
                Jt(m, !h && "x"), ue(Ce, "resize", r), M.enable()
            }, e.onDisable = function () {
                Jt(m, !0), fe(Ce, "resize", r), M.kill()
            }, ((i = new B(e)).iOS = Qe) && !x() && x(1), p = i._dc, l = Ee.to(i, {
                ease: "power4",
                paused: !0,
                scrollX: h ? "+=0.1" : "+=0",
                scrollY: "+=0.1",
                onComplete: p.vars.onComplete
            }), i
        }(e);
        return Ke && Ke.target === t.target && Ke.kill(), G(t.target) && (Ke = t), t
    }, Kt.core = {
        _getVelocityProp: f, _inputObserver: en, _scrollers: O, _proxies: A, bridge: {
            ss: function () {
                ct || Rt("scrollStart"), ct = st()
            }, ref: function () {
                return He
            }
        }
    }, q() && Ee.registerPlugin(Kt), e.ScrollTrigger = Kt, e.default = Kt, "undefined" == typeof window || window !== e ? Object.defineProperty(e, "__esModule", {value: !0}) : delete e.default
}));/*!
 * SplitText 3.6.1
 * https://greensock.com
 *
 * @license Copyright 2021, GreenSock. All rights reserved.
 * Subject to the terms at https://greensock.com/standard-license or for Club GreenSock members, the agreement issued with that membership.
 * @author: Jack Doyle, jack@greensock.com
 */
!function (D, u) {
    "object" == typeof exports && "undefined" != typeof module ? u(exports) : "function" == typeof define && define.amd ? define(["exports"], u) : u((D = D || self).window = D.window || {})
}(this, (function (D) {
    "use strict";
    var u = /([\uD800-\uDBFF][\uDC00-\uDFFF](?:[\u200D\uFE0F][\uD800-\uDBFF][\uDC00-\uDFFF]){2,}|\uD83D\uDC69(?:\u200D(?:(?:\uD83D\uDC69\u200D)?\uD83D\uDC67|(?:\uD83D\uDC69\u200D)?\uD83D\uDC66)|\uD83C[\uDFFB-\uDFFF])|\uD83D\uDC69\u200D(?:\uD83D\uDC69\u200D)?\uD83D\uDC66\u200D\uD83D\uDC66|\uD83D\uDC69\u200D(?:\uD83D\uDC69\u200D)?\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67])|\uD83C\uDFF3\uFE0F\u200D\uD83C\uDF08|(?:\uD83C[\uDFC3\uDFC4\uDFCA]|\uD83D[\uDC6E\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4-\uDEB6]|\uD83E[\uDD26\uDD37-\uDD39\uDD3D\uDD3E\uDDD6-\uDDDD])(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2640\u2642]\uFE0F|\uD83D\uDC69(?:\uD83C[\uDFFB-\uDFFF])\u200D(?:\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|(?:\uD83C[\uDFC3\uDFC4\uDFCA]|\uD83D[\uDC6E\uDC6F\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4-\uDEB6]|\uD83E[\uDD26\uDD37-\uDD39\uDD3C-\uDD3E\uDDD6-\uDDDF])\u200D[\u2640\u2642]\uFE0F|\uD83C\uDDFD\uD83C\uDDF0|\uD83C\uDDF6\uD83C\uDDE6|\uD83C\uDDF4\uD83C\uDDF2|\uD83C\uDDE9(?:\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF])|\uD83C\uDDF7(?:\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC])|\uD83C\uDDE8(?:\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF5\uDDF7\uDDFA-\uDDFF])|(?:\u26F9|\uD83C[\uDFCB\uDFCC]|\uD83D\uDD75)(?:\uFE0F\u200D[\u2640\u2642]|(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2640\u2642])\uFE0F|(?:\uD83D\uDC41\uFE0F\u200D\uD83D\uDDE8|\uD83D\uDC69(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2695\u2696\u2708]|\uD83D\uDC69\u200D[\u2695\u2696\u2708]|\uD83D\uDC68(?:(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2695\u2696\u2708]|\u200D[\u2695\u2696\u2708]))\uFE0F|\uD83C\uDDF2(?:\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF])|\uD83D\uDC69\u200D(?:\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\u2764\uFE0F\u200D(?:\uD83D\uDC8B\u200D(?:\uD83D[\uDC68\uDC69])|\uD83D[\uDC68\uDC69]))|\uD83C\uDDF1(?:\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE])|\uD83C\uDDEF(?:\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5])|\uD83C\uDDED(?:\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA])|\uD83C\uDDEB(?:\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7])|[#\*0-9]\uFE0F\u20E3|\uD83C\uDDE7(?:\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF])|\uD83C\uDDE6(?:\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF])|\uD83C\uDDFF(?:\uD83C[\uDDE6\uDDF2\uDDFC])|\uD83C\uDDF5(?:\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE])|\uD83C\uDDFB(?:\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA])|\uD83C\uDDF3(?:\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF])|\uD83C\uDFF4\uDB40\uDC67\uDB40\uDC62(?:\uDB40\uDC77\uDB40\uDC6C\uDB40\uDC73|\uDB40\uDC73\uDB40\uDC63\uDB40\uDC74|\uDB40\uDC65\uDB40\uDC6E\uDB40\uDC67)\uDB40\uDC7F|\uD83D\uDC68(?:\u200D(?:\u2764\uFE0F\u200D(?:\uD83D\uDC8B\u200D)?\uD83D\uDC68|(?:(?:\uD83D[\uDC68\uDC69])\u200D)?\uD83D\uDC66\u200D\uD83D\uDC66|(?:(?:\uD83D[\uDC68\uDC69])\u200D)?\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67])|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|(?:\uD83C[\uDFFB-\uDFFF])\u200D(?:\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]))|\uD83C\uDDF8(?:\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF])|\uD83C\uDDF0(?:\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF])|\uD83C\uDDFE(?:\uD83C[\uDDEA\uDDF9])|\uD83C\uDDEE(?:\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9])|\uD83C\uDDF9(?:\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF])|\uD83C\uDDEC(?:\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE])|\uD83C\uDDFA(?:\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF])|\uD83C\uDDEA(?:\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA])|\uD83C\uDDFC(?:\uD83C[\uDDEB\uDDF8])|(?:\u26F9|\uD83C[\uDFCB\uDFCC]|\uD83D\uDD75)(?:\uD83C[\uDFFB-\uDFFF])|(?:\uD83C[\uDFC3\uDFC4\uDFCA]|\uD83D[\uDC6E\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4-\uDEB6]|\uD83E[\uDD26\uDD37-\uDD39\uDD3D\uDD3E\uDDD6-\uDDDD])(?:\uD83C[\uDFFB-\uDFFF])|(?:[\u261D\u270A-\u270D]|\uD83C[\uDF85\uDFC2\uDFC7]|\uD83D[\uDC42\uDC43\uDC46-\uDC50\uDC66\uDC67\uDC70\uDC72\uDC74-\uDC76\uDC78\uDC7C\uDC83\uDC85\uDCAA\uDD74\uDD7A\uDD90\uDD95\uDD96\uDE4C\uDE4F\uDEC0\uDECC]|\uD83E[\uDD18-\uDD1C\uDD1E\uDD1F\uDD30-\uDD36\uDDD1-\uDDD5])(?:\uD83C[\uDFFB-\uDFFF])|\uD83D\uDC68(?:\u200D(?:(?:(?:\uD83D[\uDC68\uDC69])\u200D)?\uD83D\uDC67|(?:(?:\uD83D[\uDC68\uDC69])\u200D)?\uD83D\uDC66)|\uD83C[\uDFFB-\uDFFF])|(?:[\u261D\u26F9\u270A-\u270D]|\uD83C[\uDF85\uDFC2-\uDFC4\uDFC7\uDFCA-\uDFCC]|\uD83D[\uDC42\uDC43\uDC46-\uDC50\uDC66-\uDC69\uDC6E\uDC70-\uDC78\uDC7C\uDC81-\uDC83\uDC85-\uDC87\uDCAA\uDD74\uDD75\uDD7A\uDD90\uDD95\uDD96\uDE45-\uDE47\uDE4B-\uDE4F\uDEA3\uDEB4-\uDEB6\uDEC0\uDECC]|\uD83E[\uDD18-\uDD1C\uDD1E\uDD1F\uDD26\uDD30-\uDD39\uDD3D\uDD3E\uDDD1-\uDDDD])(?:\uD83C[\uDFFB-\uDFFF])?|(?:[\u231A\u231B\u23E9-\u23EC\u23F0\u23F3\u25FD\u25FE\u2614\u2615\u2648-\u2653\u267F\u2693\u26A1\u26AA\u26AB\u26BD\u26BE\u26C4\u26C5\u26CE\u26D4\u26EA\u26F2\u26F3\u26F5\u26FA\u26FD\u2705\u270A\u270B\u2728\u274C\u274E\u2753-\u2755\u2757\u2795-\u2797\u27B0\u27BF\u2B1B\u2B1C\u2B50\u2B55]|\uD83C[\uDC04\uDCCF\uDD8E\uDD91-\uDD9A\uDDE6-\uDDFF\uDE01\uDE1A\uDE2F\uDE32-\uDE36\uDE38-\uDE3A\uDE50\uDE51\uDF00-\uDF20\uDF2D-\uDF35\uDF37-\uDF7C\uDF7E-\uDF93\uDFA0-\uDFCA\uDFCF-\uDFD3\uDFE0-\uDFF0\uDFF4\uDFF8-\uDFFF]|\uD83D[\uDC00-\uDC3E\uDC40\uDC42-\uDCFC\uDCFF-\uDD3D\uDD4B-\uDD4E\uDD50-\uDD67\uDD7A\uDD95\uDD96\uDDA4\uDDFB-\uDE4F\uDE80-\uDEC5\uDECC\uDED0-\uDED2\uDEEB\uDEEC\uDEF4-\uDEF8]|\uD83E[\uDD10-\uDD3A\uDD3C-\uDD3E\uDD40-\uDD45\uDD47-\uDD4C\uDD50-\uDD6B\uDD80-\uDD97\uDDC0\uDDD0-\uDDE6])|(?:[#\*0-9\xA9\xAE\u203C\u2049\u2122\u2139\u2194-\u2199\u21A9\u21AA\u231A\u231B\u2328\u23CF\u23E9-\u23F3\u23F8-\u23FA\u24C2\u25AA\u25AB\u25B6\u25C0\u25FB-\u25FE\u2600-\u2604\u260E\u2611\u2614\u2615\u2618\u261D\u2620\u2622\u2623\u2626\u262A\u262E\u262F\u2638-\u263A\u2640\u2642\u2648-\u2653\u2660\u2663\u2665\u2666\u2668\u267B\u267F\u2692-\u2697\u2699\u269B\u269C\u26A0\u26A1\u26AA\u26AB\u26B0\u26B1\u26BD\u26BE\u26C4\u26C5\u26C8\u26CE\u26CF\u26D1\u26D3\u26D4\u26E9\u26EA\u26F0-\u26F5\u26F7-\u26FA\u26FD\u2702\u2705\u2708-\u270D\u270F\u2712\u2714\u2716\u271D\u2721\u2728\u2733\u2734\u2744\u2747\u274C\u274E\u2753-\u2755\u2757\u2763\u2764\u2795-\u2797\u27A1\u27B0\u27BF\u2934\u2935\u2B05-\u2B07\u2B1B\u2B1C\u2B50\u2B55\u3030\u303D\u3297\u3299]|\uD83C[\uDC04\uDCCF\uDD70\uDD71\uDD7E\uDD7F\uDD8E\uDD91-\uDD9A\uDDE6-\uDDFF\uDE01\uDE02\uDE1A\uDE2F\uDE32-\uDE3A\uDE50\uDE51\uDF00-\uDF21\uDF24-\uDF93\uDF96\uDF97\uDF99-\uDF9B\uDF9E-\uDFF0\uDFF3-\uDFF5\uDFF7-\uDFFF]|\uD83D[\uDC00-\uDCFD\uDCFF-\uDD3D\uDD49-\uDD4E\uDD50-\uDD67\uDD6F\uDD70\uDD73-\uDD7A\uDD87\uDD8A-\uDD8D\uDD90\uDD95\uDD96\uDDA4\uDDA5\uDDA8\uDDB1\uDDB2\uDDBC\uDDC2-\uDDC4\uDDD1-\uDDD3\uDDDC-\uDDDE\uDDE1\uDDE3\uDDE8\uDDEF\uDDF3\uDDFA-\uDE4F\uDE80-\uDEC5\uDECB-\uDED2\uDEE0-\uDEE5\uDEE9\uDEEB\uDEEC\uDEF0\uDEF3-\uDEF8]|\uD83E[\uDD10-\uDD3A\uDD3C-\uDD3E\uDD40-\uDD45\uDD47-\uDD4C\uDD50-\uDD6B\uDD80-\uDD97\uDDC0\uDDD0-\uDDE6])\uFE0F)/;

    function e(D) {
        return h.getComputedStyle(D)
    }

    function F(D, u) {
        var e;
        return g(D) ? D : "string" == (e = typeof D) && !u && D ? x.call(p.querySelectorAll(D), 0) : D && "object" == e && "length" in D ? x.call(D, 0) : D ? [D] : []
    }

    function t(D) {
        return "absolute" === D.position || !0 === D.absolute
    }

    function C(D, u) {
        for (var e, F = u.length; -1 < --F;) if (e = u[F], D.substr(0, e.length) === e) return e.length
    }

    function i(D, u) {
        void 0 === D && (D = "");
        var e = ~D.indexOf("++"), F = 1;
        return e && (D = D.split("++").join("")), function () {
            return "<" + u + " style='position:relative;display:inline-block;'" + (D ? " class='" + D + (e ? F++ : "") + "'>" : ">")
        }
    }

    function n(D, u, e) {
        var F = D.nodeType;
        if (1 === F || 9 === F || 11 === F) for (D = D.firstChild; D; D = D.nextSibling) n(D, u, e); else 3 !== F && 4 !== F || (D.nodeValue = D.nodeValue.split(u).join(e))
    }

    function E(D, u) {
        for (var e = u.length; -1 < --e;) D.push(u[e])
    }

    function s(D, u, e) {
        for (var F; D && D !== u;) {
            if (F = D._next || D.nextSibling) return F.textContent.charAt(0) === e;
            D = D.parentNode || D._parent
        }
    }

    function r(D) {
        var u, e, t = F(D.childNodes), C = t.length;
        for (u = 0; u < C; u++) (e = t[u])._isSplit ? r(e) : u && e.previousSibling && 3 === e.previousSibling.nodeType ? (e.previousSibling.nodeValue += 3 === e.nodeType ? e.nodeValue : e.firstChild.nodeValue, D.removeChild(e)) : 3 !== e.nodeType && (D.insertBefore(e.firstChild, e), D.removeChild(e))
    }

    function l(D, u) {
        return parseFloat(u[D]) || 0
    }

    function o(D, u, F, C, i, o, d) {
        var a, h, B, f, A, c, g, x, y, b, v, _, S = e(D), m = l("paddingLeft", S), w = -999,
            N = l("borderBottomWidth", S) + l("borderTopWidth", S),
            T = l("borderLeftWidth", S) + l("borderRightWidth", S), L = l("paddingTop", S) + l("paddingBottom", S),
            W = l("paddingLeft", S) + l("paddingRight", S), H = l("fontSize", S) * (u.lineThreshold || .2),
            O = S.textAlign, V = [], j = [], M = [], R = u.wordDelimiter || " ",
            k = u.tag ? u.tag : u.span ? "span" : "div", P = u.type || u.split || "chars,words,lines",
            q = i && ~P.indexOf("lines") ? [] : null, z = ~P.indexOf("words"), G = ~P.indexOf("chars"), I = t(u),
            J = u.linesClass, K = ~(J || "").indexOf("++"), Q = [], U = "flex" === S.display, X = D.style.display;
        for (K && (J = J.split("++").join("")), U && (D.style.display = "block"), B = (h = D.getElementsByTagName("*")).length, A = [], a = 0; a < B; a++) A[a] = h[a];
        if (q || I) for (a = 0; a < B; a++) ((c = (f = A[a]).parentNode === D) || I || G && !z) && (_ = f.offsetTop, q && c && Math.abs(_ - w) > H && ("BR" !== f.nodeName || 0 === a) && (g = [], q.push(g), w = _), I && (f._x = f.offsetLeft, f._y = _, f._w = f.offsetWidth, f._h = f.offsetHeight), q && ((f._isSplit && c || !G && c || z && c || !z && f.parentNode.parentNode === D && !f.parentNode._isSplit) && (g.push(f), f._x -= m, s(f, D, R) && (f._wordEnd = !0)), "BR" === f.nodeName && (f.nextSibling && "BR" === f.nextSibling.nodeName || 0 === a) && q.push([])));
        for (a = 0; a < B; a++) if (c = (f = A[a]).parentNode === D, "BR" !== f.nodeName) if (I && (y = f.style, z || c || (f._x += f.parentNode._x, f._y += f.parentNode._y), y.left = f._x + "px", y.top = f._y + "px", y.position = "absolute", y.display = "block", y.width = f._w + 1 + "px", y.height = f._h + "px"), !z && G) if (f._isSplit) for (f._next = h = f.nextSibling, f.parentNode.appendChild(f); h && 3 === h.nodeType && " " === h.textContent;) f._next = h.nextSibling, f.parentNode.appendChild(h), h = h.nextSibling; else f.parentNode._isSplit ? (f._parent = f.parentNode, !f.previousSibling && f.firstChild && (f.firstChild._isFirst = !0), f.nextSibling && " " === f.nextSibling.textContent && !f.nextSibling.nextSibling && Q.push(f.nextSibling), f._next = f.nextSibling && f.nextSibling._isFirst ? null : f.nextSibling, f.parentNode.removeChild(f), A.splice(a--, 1), B--) : c || (_ = !f.nextSibling && s(f.parentNode, D, R), f.parentNode._parent && f.parentNode._parent.appendChild(f), _ && f.parentNode.appendChild(p.createTextNode(" ")), "span" === k && (f.style.display = "inline"), V.push(f)); else f.parentNode._isSplit && !f._isSplit && "" !== f.innerHTML ? j.push(f) : G && !f._isSplit && ("span" === k && (f.style.display = "inline"), V.push(f)); else q || I ? (f.parentNode && f.parentNode.removeChild(f), A.splice(a--, 1), B--) : z || D.appendChild(f);
        for (a = Q.length; -1 < --a;) Q[a].parentNode.removeChild(Q[a]);
        if (q) {
            for (I && (b = p.createElement(k), D.appendChild(b), v = b.offsetWidth + "px", _ = b.offsetParent === D ? 0 : D.offsetLeft, D.removeChild(b)), y = D.style.cssText, D.style.cssText = "display:none;"; D.firstChild;) D.removeChild(D.firstChild);
            for (x = " " === R && (!I || !z && !G), a = 0; a < q.length; a++) {
                for (g = q[a], (b = p.createElement(k)).style.cssText = "display:block;text-align:" + O + ";position:" + (I ? "absolute;" : "relative;"), J && (b.className = J + (K ? a + 1 : "")), M.push(b), B = g.length, h = 0; h < B; h++) "BR" !== g[h].nodeName && (f = g[h], b.appendChild(f), x && f._wordEnd && b.appendChild(p.createTextNode(" ")), I && (0 === h && (b.style.top = f._y + "px", b.style.left = m + _ + "px"), f.style.top = "0px", _ && (f.style.left = f._x - _ + "px")));
                0 === B ? b.innerHTML = "&nbsp;" : z || G || (r(b), n(b, String.fromCharCode(160), " ")), I && (b.style.width = v, b.style.height = f._h + "px"), D.appendChild(b)
            }
            D.style.cssText = y
        }
        I && (d > D.clientHeight && (D.style.height = d - L + "px", D.clientHeight < d && (D.style.height = d + N + "px")), o > D.clientWidth && (D.style.width = o - W + "px", D.clientWidth < o && (D.style.width = o + T + "px"))), U && (X ? D.style.display = X : D.style.removeProperty("display")), E(F, V), z && E(C, j), E(i, M)
    }

    function d(D, e, F, i) {
        var E, s, r, l, o, d, a, h, B = e.tag ? e.tag : e.span ? "span" : "div",
            f = ~(e.type || e.split || "chars,words,lines").indexOf("chars"), g = t(e), x = e.wordDelimiter || " ",
            y = " " !== x ? "" : g ? "&#173; " : " ", b = "</" + B + ">", v = 1,
            _ = e.specialChars ? "function" == typeof e.specialChars ? e.specialChars : C : null,
            S = p.createElement("div"), m = D.parentNode;
        for (m.insertBefore(S, D), S.textContent = D.nodeValue, m.removeChild(D), a = -1 !== (E = function D(u) {
            var e = u.nodeType, F = "";
            if (1 === e || 9 === e || 11 === e) {
                if ("string" == typeof u.textContent) return u.textContent;
                for (u = u.firstChild; u; u = u.nextSibling) F += D(u)
            } else if (3 === e || 4 === e) return u.nodeValue;
            return F
        }(D = S)).indexOf("<"), !1 !== e.reduceWhiteSpace && (E = E.replace(c, " ").replace(A, "")), a && (E = E.split("<").join("{{LT}}")), o = E.length, s = (" " === E.charAt(0) ? y : "") + F(), r = 0; r < o; r++) if (d = E.charAt(r), _ && (h = _(E.substr(r), e.specialChars))) d = E.substr(r, h || 1), s += f && " " !== d ? i() + d + "</" + B + ">" : d, r += h - 1; else if (d === x && E.charAt(r - 1) !== x && r) {
            for (s += v ? b : "", v = 0; E.charAt(r + 1) === x;) s += y, r++;
            r === o - 1 ? s += y : ")" !== E.charAt(r + 1) && (s += y + F(), v = 1)
        } else "{" === d && "{{LT}}" === E.substr(r, 6) ? (s += f ? i() + "{{LT}}</" + B + ">" : "{{LT}}", r += 5) : 55296 <= d.charCodeAt(0) && d.charCodeAt(0) <= 56319 || 65024 <= E.charCodeAt(r + 1) && E.charCodeAt(r + 1) <= 65039 ? (l = ((E.substr(r, 12).split(u) || [])[1] || "").length || 2, s += f && " " !== d ? i() + E.substr(r, l) + "</" + B + ">" : E.substr(r, l), r += l - 1) : s += f && " " !== d ? i() + d + "</" + B + ">" : d;
        D.outerHTML = s + (v ? b : ""), a && n(m, "{{LT}}", "<")
    }

    function a(D, u, C, i) {
        var n, E, s = F(D.childNodes), r = s.length, l = t(u);
        if (3 !== D.nodeType || 1 < r) {
            for (u.absolute = !1, n = 0; n < r; n++) (E = s[n])._next = E._isFirst = E._parent = E._wordEnd = null, 3 === E.nodeType && !/\S+/.test(E.nodeValue) || (l && 3 !== E.nodeType && "inline" === e(E).display && (E.style.display = "inline-block", E.style.position = "relative"), E._isSplit = !0, a(E, u, C, i));
            return u.absolute = l, void (D._isSplit = !0)
        }
        d(D, u, C, i)
    }

    var p, h, B, f, A = /(?:\r|\n|\t\t)/g, c = /(?:\s\s+)/g, g = Array.isArray, x = [].slice,
        y = ((f = b.prototype).split = function (D) {
            this.isSplit && this.revert(), this.vars = D = D || this.vars, this._originals.length = this.chars.length = this.words.length = this.lines.length = 0;
            for (var u, e, F, t = this.elements.length, C = D.tag ? D.tag : D.span ? "span" : "div", n = i(D.wordsClass, C), E = i(D.charsClass, C); -1 < --t;) F = this.elements[t], this._originals[t] = F.innerHTML, u = F.clientHeight, e = F.clientWidth, a(F, D, n, E), o(F, D, this.chars, this.words, this.lines, e, u);
            return this.chars.reverse(), this.words.reverse(), this.lines.reverse(), this.isSplit = !0, this
        }, f.revert = function () {
            var D = this._originals;
            if (!D) throw "revert() call wasn't scoped properly.";
            return this.elements.forEach((function (u, e) {
                return u.innerHTML = D[e]
            })), this.chars = [], this.words = [], this.lines = [], this.isSplit = !1, this
        }, b.create = function (D, u) {
            return new b(D, u)
        }, b);

    function b(D, u) {
        B || (p = document, h = window, B = 1), this.elements = F(D), this.chars = [], this.words = [], this.lines = [], this._originals = [], this.vars = u || {}, this.split(u)
    }

    y.version = "3.6.1", D.SplitText = y, D.default = y, "undefined" == typeof window || window !== D ? Object.defineProperty(D, "__esModule", {value: !0}) : delete D.default
}));