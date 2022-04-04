/*
jQWidgets v2.8.0 (2013-Mar-22)
Copyright (c) 2011-2013 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a){a.extend(a.jqx._jqxGrid.prototype,{exportdata:function(j,q,p,i,k,m,d){if(!a.jqx.dataAdapter.ArrayExporter){throw"jqxdata.export.js is not loaded."}if(p==undefined){p=true}var w=this;if(i==undefined){var i=this.getrows();if(i.length==0){throw"No data to export."}}var u=k!=undefined?k:false;var t={};var h={};var n=[];var g=this.host.find(".jqx-grid-cell:first");var o=this.host.find(".jqx-grid-cell-alt:first");g.removeClass(this.toThemeProperty("jqx-grid-cell-selected"));g.removeClass(this.toThemeProperty("jqx-fill-state-pressed"));o.removeClass(this.toThemeProperty("jqx-grid-cell-selected"));o.removeClass(this.toThemeProperty("jqx-fill-state-pressed"));g.removeClass(this.toThemeProperty("jqx-grid-cell-hover"));g.removeClass(this.toThemeProperty("jqx-fill-state-hover"));o.removeClass(this.toThemeProperty("jqx-grid-cell-hover"));o.removeClass(this.toThemeProperty("jqx-fill-state-hover"));var e="cell";var c=1;var v="column";var b=1;var f=[];a.each(this.columns.records,function(z){var D=a(w.table[0].rows[0].cells[z]);if(w.table[0].rows.length>1){var A=a(w.table[0].rows[1].cells[z])}var C=function(E){E.removeClass(w.toThemeProperty("jqx-grid-cell-selected"));E.removeClass(w.toThemeProperty("jqx-fill-state-pressed"));E.removeClass(w.toThemeProperty("jqx-grid-cell-hover"));E.removeClass(w.toThemeProperty("jqx-fill-state-hover"))};C(D);if(A){C(A)}if(this.datafield==null){return true}if(w.showaggregates){if(w.getcolumnaggregateddata){f.push(w.getcolumnaggregateddata(this.datafield,this.aggregates,true,i))}}var B=w._getexportcolumntype(this);if(this.exportable&&(!this.hidden||u)){t[this.datafield]={};t[this.datafield].text=this.text;t[this.datafield].width=parseInt(this.width);if(isNaN(t[this.datafield].width)){t[this.datafield].width=60}t[this.datafield].formatString=this.cellsformat;t[this.datafield].localization=w.gridlocalization;t[this.datafield].type=B;t[this.datafield].cellsAlign=this.cellsalign;t[this.datafield].hidden=!p}e="cell"+c;var x=a(this.element);if(this.element==undefined){x=a(this.uielement)}v="column"+b;if(j=="html"||j=="xls"||j=="pdf"){var y=function(I,F,G,E,K,J,H){h[I]={};h[I]["font-size"]=F.css("font-size");h[I]["font-weight"]=F.css("font-weight");h[I]["font-style"]=F.css("font-style");h[I]["background-color"]=J._getexportcolor(F.css("background-color"));h[I]["color"]=J._getexportcolor(F.css("color"));h[I]["border-color"]=J._getexportcolor(F.css("border-top-color"));if(G){h[I]["text-align"]=K.align}else{h[I]["text-align"]=K.cellsalign;h[I]["formatString"]=K.cellsformat;h[I]["dataType"]=B}if(j=="html"||j=="pdf"){h[I]["border-top-width"]=F.css("border-top-width");h[I]["border-left-width"]=F.css("border-left-width");h[I]["border-right-width"]=F.css("border-right-width");h[I]["border-bottom-width"]=F.css("border-bottom-width");h[I]["border-top-style"]=F.css("border-top-style");h[I]["border-left-style"]=F.css("border-left-style");h[I]["border-right-style"]=F.css("border-right-style");h[I]["border-bottom-style"]=F.css("border-bottom-style");if(G){if(H==0){h[I]["border-left-width"]=F.css("border-right-width")}h[I]["border-top-width"]=F.css("border-right-width");h[I]["border-bottom-width"]=F.css("border-bottom-width")}else{if(H==0){h[I]["border-left-width"]=F.css("border-right-width")}}h[I]["height"]=F.css("height")}if(K.exportable&&(!K.hidden||u)){if(G){t[K.datafield].style=I}else{if(!E){t[K.datafield].cellStyle=I}else{t[K.datafield].cellAltStyle=I}}}};y(v,x,true,false,this,w,z);b++;y(e,D,false,false,this,w,z);if(w.altrows){e="cellalt"+c;y(e,A,false,true,this,w,z)}c++}});if(this.showaggregates){var s=[];var r=j=="xls"?"AG":"";if(f.length>0){a.each(this.columns.records,function(x){if(this.aggregates){for(var z=0;z<this.aggregates.length;z++){if(!s[z]){s[z]={}}if(s[z]){var A=w._getaggregatename(this.aggregates[z]);var B=w._getaggregatetype(this.aggregates[z]);var y=f[x];s[z][this.datafield]=r+A+": "+y[B]}}}});a.each(this.columns.records,function(x){for(var y=0;y<s.length;y++){if(s[y][this.datafield]==undefined){s[y][this.datafield]=r}}})}a.each(s,function(){i.push(this)})}var l=a.jqx.dataAdapter.ArrayExporter(i,t,h);if(q==undefined){this._renderrows(this.virtualsizeinfo);return l.exportTo(j)}else{l.exportToFile(j,q,m,d)}if(this.showaggregates){a.each(s,function(){i.pop(this)})}this._renderrows(this.virtualsizeinfo)},_getexportcolor:function(k){var f=k;if(k=="transparent"){f="#FFFFFF"}if(!f||!f.toString()){f="#FFFFFF"}if(f.toString().indexOf("rgb")!=-1){var i=f.split(",");var d=parseInt(i[0].substring(4));var h=parseInt(i[1]);var j=parseInt(i[2].substring(1,4));var l={r:d,g:h,b:j};var e=this._rgbToHex(l);return"#"+e}else{if(f.toString().indexOf("#")!=-1){if(f.toString().length==4){var c=f.toString().substring(1,4);f+=c}}}return f},_rgbToHex:function(b){return this._intToHex(b.r)+this._intToHex(b.g)+this._intToHex(b.b)},_intToHex:function(c){var b=(parseInt(c).toString(16));if(b.length==1){b=("0"+b)}return b.toUpperCase()},_getexportcolumntype:function(e){var f=this;var d="string";var c=f.source.datafields||((f.source._source)?f.source._source.datafields:null);if(c){var h="";a.each(c,function(){if(this.name==e.datafield){if(this.type){h=this.type}return false}});if(h){return h}}if(e!=null){if(this.dataview.cachedrecords==undefined){return d}var b=null;if(!this.virtualmode){if(this.dataview.cachedrecords.length==0){return d}b=this.dataview.cachedrecords[0][e.datafield];if(b!=null&&b.toString()==""){return"string"}}else{a.each(this.dataview.cachedrecords,function(){b=this[e.datafield];return false})}if(b!=null){if(e.cellsformat.indexOf("c")!=-1){return"number"}if(e.cellsformat.indexOf("n")!=-1){return"number"}if(e.cellsformat.indexOf("p")!=-1){return"number"}if(e.cellsformat.indexOf("d")!=-1){return"date"}if(e.cellsformat.indexOf("y")!=-1){return"date"}if(e.cellsformat.indexOf("M")!=-1){return"date"}if(e.cellsformat.indexOf("m")!=-1){return"date"}if(e.cellsformat.indexOf("t")!=-1){return"date"}if(typeof b=="boolean"){d="boolean"}else{if(a.jqx.dataFormat.isNumber(b)){d="number"}else{var g=new Date(b);if(g.toString()=="NaN"||g.toString()=="Invalid Date"){if(a.jqx.dataFormat){g=a.jqx.dataFormat.tryparsedate(b);if(g!=null){return"date"}else{d="string"}}else{d="string"}}else{d="date"}}}}}return d}})})(jQuery);