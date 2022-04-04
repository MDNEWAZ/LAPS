/*
jQWidgets v2.8.0 (2013-Mar-22)
Copyright (c) 2011-2013 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a){a.extend(a.jqx._jqxGrid.prototype,{autoresizecolumns:function(t){if(t!="cells"&&t!="all"&&t!="column"){t="all"}var u=this;var k=this.getrows();if(this.pageable){k=this.dataview.rows}var e=k.length;if(e==undefined&&k!=undefined){var o=new Array();a.each(k,function(i){o.push(this)});k=o;e=k.length}var q=a("<span></span>");q.addClass("jqx-grid-cell");a(document.body).append(q);var c=[];var f=[];var b=[];var l=u.host.width();if(u.vScrollBar[0].style.visibility!="hidden"){l-=this.scrollbarsize+5}if(l<0){l=0}for(var r=0;r<e;r++){var h=k[r];for(var p=0;p<this.columns.records.length;p++){var d=this.columns.records[p];if(d.hidden){continue}if(f[d.displayfield]==undefined){f[d.displayfield]=0}if(b[d.displayfield]==undefined){b[d.displayfield]=""}var n=h[d.displayfield];if(d.cellsformat!=""){if(a.jqx.dataFormat){if(a.jqx.dataFormat.isDate(n)){n=a.jqx.dataFormat.formatdate(n,d.cellsformat,this.gridlocalization)}else{if(a.jqx.dataFormat.isNumber(n)){n=a.jqx.dataFormat.formatnumber(n,d.cellsformat,this.gridlocalization)}}}}else{if(d.cellsrenderer){var m=d.cellsrenderer(r,d,n);if(m!=undefined){n=m.toString()}}}if(t==undefined||t=="cells"||t=="all"){if(n!=null){var c=n.toString().length;if(c>f[d.displayfield]){f[d.displayfield]=c;b[d.displayfield]=n}}}if(t=="column"||t=="all"){if(d.text.toString().length>f[d.displayfield]){b[d.displayfield]=d.text}}}}for(var p=0;p<this.columns.records.length;p++){var d=this.columns.records[p];if(b[d.displayfield]==undefined){b[d.displayfield]=d.text}q[0].innerHTML=b[d.displayfield];var s=q.outerWidth()+10;if(q.children().length>0){s=q.children().outerWidth()+10}if(a.jqx.browser.msie&&a.jqx.browser.version<8){s+=10}if(s>d.maxwidth){s=d.maxwidth}if(d._width!=undefined){d.__width=d._width}d._width=null;if(d.maxwidth=="auto"||s<=d.maxwidth){var g=d.width;d.width=s;if(d._percentagewidth!=undefined){d._percentagewidth=(d.width/l)*100}this._raiseEvent(14,{columntext:d.text,column:d.getcolumnproperties(),datafield:d.datafield,displayfield:d.displayfield,oldwidth:g,newwidth:s})}}q.remove();this._updatecolumnwidths();this._updatecellwidths();this._renderrows(this.virtualsizeinfo);for(var p=0;p<this.columns.records.length;p++){var d=this.columns.records[p];if(d.__width!=undefined){d._width=d.__width}}},autoresizecolumn:function(f,b){if(b!="cells"&&b!="all"&&b!="column"){b="all"}if(f==undefined){return false}var r=this.getrows();if(this.pageable){r=this.dataview.rows}var g=this.getcolumn(f);if(g==undefined){return false}var e=r.length;var m=a("<span></span>");m.addClass("jqx-grid-cell");a(document.body).append(m);var d=0;var l="";var k=this;var c=k.host.width();if(k.vScrollBar[0].style.visibility!="hidden"){c-=this.scrollbarsize+5}if(c<0){c=0}if(b==undefined||b=="cells"||b=="all"){for(var h=0;h<e;h++){var o=r[h][g.displayfield];if(g.cellsformat!=""){if(a.jqx.dataFormat){if(a.jqx.dataFormat.isDate(o)){o=a.jqx.dataFormat.formatdate(o,g.cellsformat,this.gridlocalization)}else{if(a.jqx.dataFormat.isNumber(o)){o=a.jqx.dataFormat.formatnumber(o,g.cellsformat,this.gridlocalization)}}}}else{if(g.cellsrenderer){var q=g.cellsrenderer(h,g,o);if(q!=undefined){o=q.toString()}}}if(o!=null){var p=o.toString().length;if(p>d){d=p;l=o}}}}if(b=="column"||b=="all"){if(g.text.toString().length>d){l=g.text}}if(l==undefined){l=g.text}m[0].innerHTML=l;var n=m.outerWidth()+10;if(a.jqx.browser.msie&&a.jqx.browser.version<8){n+=5}m.remove();if(n>g.maxwidth){n=g.maxwidth}if(g.maxwidth=="auto"||n<=g.maxwidth){var j=g.width;g.width=n;if(g._width!=undefined){g.__width=g._width}g._width=null;if(g._percentagewidth!=undefined){g._percentagewidth=(g.width/c)*100}this._updatecolumnwidths();this._updatecellwidths();this._raiseEvent(14,{columntext:g.text,column:g.getcolumnproperties(),datafield:f,displayfield:g.displayfield,oldwidth:j,newwidth:n});this._renderrows(this.virtualsizeinfo);if(g._width!=undefined){g._width=g.__width}}},_handlecolumnsresize:function(){var j=this;if(this.columnsresize){var i=false;if(j.isTouchDevice()){i=true}var f="mousemove.resize"+this.element.id;var c="mousedown.resize"+this.element.id;var d="mouseup.resize"+this.element.id;if(i){var f=a.jqx.mobile.getTouchEventName("touchmove")+".resize"+this.element.id;var c=a.jqx.mobile.getTouchEventName("touchstart")+".resize"+this.element.id;var d=a.jqx.mobile.getTouchEventName("touchend")+".resize"+this.element.id}this.removeHandler(a(document),f);this.addHandler(a(document),f,function(m){var n=a.data(document.body,"contextmenu"+j.element.id);if(n!=null){return true}if(j.resizablecolumn!=null&&!j.disabled&&j.resizing){if(j.resizeline!=null){var p=j.host.coord();var u=parseInt(j.resizestartline.coord().left);var k=u-j._startcolumnwidth;var v=j.resizablecolumn.column.minwidth;if(v=="auto"){v=0}else{v=parseInt(v)}var l=j.resizablecolumn.column.maxwidth;if(l=="auto"){l=0}else{l=parseInt(l)}var q=m.pageX;if(i){var s=j.getTouches(m);var r=s[0];q=r.pageX}k+=v;var t=l>0?u+l:0;var o=l==0?true:j._startcolumnwidth+q-u<l?true:false;if(j.rtl){var o=true}if(o){if(!j.rtl){if(q>=p.left&&q>=k&&q<=p.left+j.host.width()){if(t!=0&&m.pageX<t){j.resizeline.css("left",q)}else{if(t==0){j.resizeline.css("left",q)}}if(i){return false}}}else{if(q>=p.left&&q<=p.left+j.host.width()){j.resizeline.css("left",q);if(i){return false}}}}}}if(!i&&j.resizablecolumn!=null){return false}});this.removeHandler(a(document),c);this.addHandler(a(document),c,function(m){var l=a.data(document.body,"contextmenu"+j.element.id);if(l!=null){return true}if(j.resizablecolumn!=null&&!j.disabled){var k=j.resizablecolumn.columnelement;if(k.coord().top+k.height()+5<m.pageY){j.resizablecolumn=null;return}j._startcolumnwidth=j.resizablecolumn.column.width;j.resizablecolumn.column._width=null;a(document.body).addClass("jqx-disableselect");j._mouseDownResize=new Date();j.resizing=true;j._resizecolumn=j.resizablecolumn.column;j.resizeline=j.resizeline||a('<div style="position: absolute;"></div>');j.resizestartline=j.resizestartline||a('<div style="position: absolute;"></div>');j.resizebackground=j.resizebackground||a('<div style="position: absolute; left: 0; top: 0; background: #000;"></div>');j.resizebackground.css("opacity",0.01);j.resizebackground.css("cursor","col-resize");j.resizeline.css("cursor","col-resize");j.resizestartline.css("cursor","col-resize");j.resizeline.addClass(j.toThemeProperty("jqx-grid-column-resizeline"));j.resizestartline.addClass(j.toThemeProperty("jqx-grid-column-resizestartline"));a(document.body).append(j.resizeline);a(document.body).append(j.resizestartline);a(document.body).append(j.resizebackground);var n=j.resizablecolumn.columnelement.coord();j.resizebackground.css("left",j.host.coord().left);j.resizebackground.css("top",j.host.coord().top);j.resizebackground.width(j.host.width());j.resizebackground.height(j.host.height());j.resizebackground.css("z-index",999999999);var o=function(q){if(!j.rtl){q.css("left",parseInt(n.left)+j._startcolumnwidth)}else{q.css("left",parseInt(n.left))}var t=j._groupsheader();var s=t?j.groupsheader.height():0;var v=j.showtoolbar?j.toolbarheight:0;s+=v;var p=j.showstatusbar?j.statusbarheight:0;s+=p;var r=0;if(j.pageable){r=j.pagerheight}var u=j.hScrollBar.css("visibility")=="visible"?17:0;q.css("top",parseInt(n.top));q.css("z-index",9999999999);q.height(j.host.height()-r-s-u);if(j.enableanimations){q.show("fast")}else{q.show()}};o(j.resizeline);o(j.resizestartline);j.dragmousedown=null}});var e=function(){a(document.body).removeClass("jqx-disableselect");if(!j.resizing){return}j._mouseUpResize=new Date();var o=j._mouseUpResize-j._mouseDownResize;if(o<200){j.resizing=false;if(j._resizecolumn!=null&&j.resizeline!=null&&j.resizeline.css("display")=="block"){j._resizecolumn=null;j.resizeline.hide();j.resizestartline.hide();j.resizebackground.remove()}return}j.resizing=false;if(j.disabled){return}var m=j.host.width();if(j.vScrollBar[0].style.visibility!="hidden"){m-=20}if(m<0){m=0}if(j._resizecolumn!=null&&j.resizeline!=null&&j.resizeline.css("display")=="block"){var p=parseInt(j.resizeline.css("left"));var l=parseInt(j.resizestartline.css("left"));var k=j._startcolumnwidth+p-l;if(j.rtl){var k=j._startcolumnwidth-p+l}var n=j._resizecolumn.width;j._closemenu();j._resizecolumn.width=k;if(j._resizecolumn._percentagewidth!=undefined){j._resizecolumn._percentagewidth=(k/m)*100}j._updatecolumnwidths();j._updatecellwidths();j._raiseEvent(14,{columntext:j._resizecolumn.text,column:j._resizecolumn.getcolumnproperties(),datafield:j._resizecolumn.datafield,oldwidth:n,newwidth:k});j._renderrows(j.virtualsizeinfo);if(j.autosavestate){if(j.savestate){j.savestate()}}j._resizecolumn=null;j.resizeline.hide();j.resizestartline.hide();j.resizebackground.remove();j.resizablecolumn=null}else{j.resizablecolumn=null}};try{if(document.referrer!=""||window.frameElement){var b=null;if(window.top!=null&&window.top!=window.self){if(window.parent&&document.referrer){b=document.referrer}}if(b&&b.indexOf(document.location.host)!=-1){var g=function(k){e()};if(window.top.document.addEventListener){window.top.document.addEventListener("mouseup",g,false)}else{if(window.top.document.attachEvent){window.top.document.attachEvent("onmouseup",g)}}}}}catch(h){}this.removeHandler(a(document),d);this.addHandler(a(document),d,function(l){var k=a.data(document.body,"contextmenu"+j.element.id);if(k!=null){return true}e()})}}})})(jQuery);