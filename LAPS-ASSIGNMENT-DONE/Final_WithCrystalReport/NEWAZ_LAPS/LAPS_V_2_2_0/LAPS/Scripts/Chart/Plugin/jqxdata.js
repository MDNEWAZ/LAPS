/*
jQWidgets v2.8.0 (2013-Mar-22)
Copyright (c) 2011-2013 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a){a.jqx.dataAdapter=function(e,b){this._source=e;this._options=b||{};this.records=new Array();this._downloadComplete=new Array();this._bindingUpdate=new Array();if(e!=undefined&&e.localdata!=null&&typeof e.localdata=="function"){var d=e.localdata();if(d!=null){e._localdata=e.localdata;var c=this;if(e._localdata.subscribe){c._oldlocaldata=[];e._localdata.subscribe(function(f){var g=function(h){if(jQuery.isArray(h)){return jQuery.makeArray(g(a(h)))}return jQuery.extend(true,{},h)};c._oldlocaldata=g(f)},e._localdata,"beforeChange");e._localdata.subscribe(function(g){if(c.suspendKO==false||c.suspendKO==undefined){var f="";c._oldrecords=c.records;if(c._oldlocaldata.length==0){e.localdata=e._localdata()}if(c._oldlocaldata.length==0){f="change"}else{if(c._oldlocaldata.length==g.length){f="update"}if(c._oldlocaldata.length>g.length){f="remove"}if(c._oldlocaldata.length<g.length){f="add"}}c.dataBind(null,f)}},e._localdata,"change");c._knockoutdatasource=true}e.localdata=d}}if(this._options.autoBind==true){this.dataBind()}};a.jqx.dataAdapter.prototype={getrecords:function(){return this.records},beginUpdate:function(){this.isUpdating=true},endUpdate:function(b){this.isUpdating=false;if(b!=false){this.dataBind(null,"")}},formatDate:function(c,e,d){var b=a.jqx.dataFormat.formatdate(c,e,d);return b},formatNumber:function(c,e,d){var b=a.jqx.dataFormat.formatnumber(c,e,d);return b},dataBind:function(n,s){if(this.isUpdating==true){return}var p=this._source;if(!p){return}if(p.dataFields!=null){p.datafields=p.dataFields}if(p.recordstartindex==undefined){p.recordstartindex=0}if(p.recordendindex==undefined){p.recordendindex=0}if(p.loadallrecords==undefined){p.loadallrecords=true}if(p.sort!=undefined){this.sort=p.sort}if(p.filter!=undefined){this.filter=p.filter}else{this.filter=null}if(p.sortcolumn!=undefined){this.sortcolumn=p.sortcolumn}if(p.sortdirection!=undefined){this.sortdirection=p.sortdirection}if(p.sortcomparer!=undefined){this.sortcomparer=p.sortcomparer}this.records=new Array();var f=this._options||{};this.virtualmode=f.virtualmode!=undefined?f.virtualmode:false;this.totalrecords=f.totalrecords!=undefined?f.totalrecords:0;this.pageable=f.pageable!=undefined?f.pageable:false;this.pagesize=f.pagesize!=undefined?f.pagesize:0;this.pagenum=f.pagenum!=undefined?f.pagenum:0;this.cachedrecords=f.cachedrecords!=undefined?f.cachedrecords:new Array();this.originaldata=new Array();this.recordids=new Array();this.updaterow=f.updaterow!=undefined?f.updaterow:null;this.addrow=f.addrow!=undefined?f.addrow:null;this.deleterow=f.deleterow!=undefined?f.deleterow:null;this.cache=f.cache!=undefined?f.cache:false;this.unboundmode=false;if(p.formatdata!=undefined){f.formatData=p.formatdata}if(p.data!=undefined){if(f.data==undefined){f.data={}}a.extend(f.data,p.data)}if(p.mapchar!=undefined){this.mapChar=p.mapchar?p.mapchar:">"}else{this.mapChar=f.mapChar?f.mapChar:">"}if(f.unboundmode||p.unboundmode){this.unboundmode=f.unboundmode||p.unboundmode}if(p.cache!=undefined){this.cache=p.cache}if(this.koSubscriptions){for(var u=0;u<this.koSubscriptions.length;u++){this.koSubscriptions[u].dispose()}}this.koSubscriptions=new Array();if(this.pagenum<0){this.pagenum=0}var z=this;var m=p.datatype;if(p.datatype==="csv"||p.datatype==="tab"||p.datatype=="text"){m="text"}var h=f.async!=undefined?f.async:true;if(p.async!=undefined){h=p.async}switch(m){case"local":case"array":case"observablearray":default:if(p.localdata==undefined&&p.length){p.localdata=new Array();for(var r=0;r<p.length;r++){p.localdata[p.localdata.length]=p[r]}}var g=p.localdata.length;this.totalrecords=this.virtualmode?(p.totalrecords||g):g;if(this.unboundmode){this.totalrecords=this.unboundmode?(p.totalrecords||g):g;var v=p.datafields?p.datafields.length:0;if(v>0){for(var r=0;r<this.totalrecords;r++){var d={};for(var q=0;q<v;q++){d[p.datafields[q].name]=""}p.localdata[p.localdata.length]=d}}}if(this.totalrecords==undefined){this.totalrecords=0}var v=p.datafields?p.datafields.length:0;var c=function(E,G){var F={};for(var C=0;C<G;C++){var B=p.datafields[C];var H="";if(undefined==B||B==null){continue}if(B.map){var i=B.map.split(z.mapChar);if(i.length>0){var D=E;for(var A=0;A<i.length;A++){D=D[i[A]]}H=D}else{H=E[B.map]}if(H!=undefined&&H!=null){H=H.toString()}else{H=""}}if(H==""){H=E[B.name];if(H!=undefined&&H!=null){if(p._localdata&&H.subscribe){H=H()}else{H=H.toString()}}else{H=""}}H=z.getvaluebytype(H,B);if(B.displayname!=undefined){F[B.displayname]=H}else{F[B.name]=H}}return F};if(p._localdata){this._changedrecords=[];this.records=new Array();var y=p._localdata();a.each(y,function(J,O){if(typeof O==="string"){z.records.push(O)}else{var L={};var M=0;for(var I in this){var C=null;var N="string";if(v>0){var P=false;for(var G=0;G<v;G++){var F=p.datafields[G];if(F!=undefined&&F.name==I){P=true;C=F.map;N=F.type;break}}if(!P){continue}}var E=a.isFunction(this[I]);if(E){var O=this[I]();if(N!="string"){O=z.getvaluebytype(O,{type:N})}L[I]=O;if(this[I].subscribe){z.koSubscriptions[z.koSubscriptions.length]=this[I].subscribe(function(i){z.dataBind(null,null);return false})}}else{var O=this[I];if(C!=null){var B=C.split(z.mapChar);if(B.length>0){var K=this;for(var D=0;D<B.length;D++){K=K[B[D]]}O=K}else{O=this[C]}}if(N!="string"){O=z.getvaluebytype(O,{type:N})}L[I]=O;if(L[I]!=undefined){M+=L[I].toString().length+L[I].toString().substr(0,1)}}}z.records.push(L);L._koindex=M;if(z._oldrecords){var H=z.records.length-1;if(s=="update"){if(z._oldrecords[H]._koindex!=M){var A={index:H,oldrecord:z._oldrecords[H],record:L};z._changedrecords.push(A)}}}}});if(s=="add"){var g=z.records.length;for(var r=0;r<g;r++){var d=z.records[r];if(!z._oldrecords[r]){z._changedrecords.push({index:r,oldrecord:null,record:d})}else{if(z._oldrecords[r]._koindex!=d._koindex){z._changedrecords.push({index:r,oldrecord:null,record:d})}}}}else{if(s=="remove"){var g=z._oldrecords.length;for(var r=0;r<g;r++){var l=z._oldrecords[r];if(!z.records[r]){z._changedrecords.push({index:r,oldrecord:l,record:null})}else{if(z.records[r]._koindex!=l._koindex){z._changedrecords.push({index:r,oldrecord:l,record:null})}}}}}}else{if(!a.isArray(p.localdata)){this.records=new Array();a.each(p.localdata,function(A){if(v>0){var j=this;var B=c(j,v);z.records[z.records.length]=B}else{z.records[z.records.length]=this}})}else{if(v==0){this.records=p.localdata}else{a.each(p.localdata,function(B){var j=this;var C=c(j,v);var A=z.getid(p.id,C,B);if(typeof(A)==="object"){A=B}var j=a.extend({},C);j.uid=A;z.records[z.records.length]=j})}}}this.originaldata=p.localdata;this.cachedrecords=this.records;this.addForeignValues(p);if(f.uniqueDataFields){var o=this.getUniqueRecords(this.records,f.uniqueDataFields);this.records=o;this.cachedrecords=o}if(f.beforeLoadComplete){var w=f.beforeLoadComplete(z.records,this.originaldata);if(w!=undefined){z.records=w;z.cachedrecords=w}}if(f.autoSort&&f.autoSortField){var k=Object.prototype.toString;Object.prototype.toString=(typeof field=="function")?field:function(){return this[f.autoSortField]};z.records.sort();Object.prototype.toString=k}if(a.isFunction(f.loadComplete)){f.loadComplete(p.localdata)}break;case"json":case"jsonp":case"xml":case"xhtml":case"script":case"text":if(p.localdata!=null){if(a.isFunction(p.beforeprocessing)){p.beforeprocessing(p.localdata)}if(p.datatype==="xml"){z.loadxml(p.localdata,p.localdata,p)}else{if(m==="text"){z.loadtext(p.localdata,p)}else{z.loadjson(p.localdata,p.localdata,p)}}z.addForeignValues(p);if(f.uniqueDataFields){var o=z.getUniqueRecords(z.records,f.uniqueDataFields);z.records=o;z.cachedrecords=o}if(a.isFunction(f.loadComplete)){f.loadComplete(p.localdata)}return}var x=f.data!=undefined?f.data:{};if(p.processdata){p.processdata(x)}if(a.isFunction(f.processData)){f.processData(x)}if(a.isFunction(f.formatData)){var b=f.formatData(x);if(b!=undefined){x=b}}var t="application/x-www-form-urlencoded";if(f.contentType){t=f.contentType}var e="GET";if(p.type){e=p.type}if(f.type){e=f.type}if(p.url&&p.url.length>0){if(a.isFunction(f.loadServerData)){z._requestData(x,p,f)}else{this.xhr=a.ajax({dataType:m,cache:this.cache,type:e,url:p.url,async:h,contentType:t,data:x,success:function(B,i,E){if(a.isFunction(p.beforeprocessing)){var D=p.beforeprocessing(B,i,E);if(D!=undefined){B=D}}if(a.isFunction(f.downloadComplete)){var D=f.downloadComplete(B,i,E);if(D!=undefined){B=D}}if(B==null){z.records=new Array();z.cachedrecords=new Array();z.originaldata=new Array();z.callDownloadComplete();if(a.isFunction(f.loadComplete)){f.loadComplete(new Array())}return}var j=B;if(B.records){j=B.records}if(B.totalrecords!=undefined){p.totalrecords=B.totalrecords}if(p.datatype==="xml"){z.loadxml(null,j,p)}else{if(m==="text"){z.loadtext(j,p)}else{z.loadjson(null,j,p)}}z.addForeignValues(p);if(f.uniqueDataFields){var A=z.getUniqueRecords(z.records,f.uniqueDataFields);z.records=A;z.cachedrecords=A}if(f.beforeLoadComplete){var C=f.beforeLoadComplete(z.records,B);if(C!=undefined){z.records=C;z.cachedrecords=C}}z.callDownloadComplete();if(a.isFunction(f.loadComplete)){f.loadComplete(B)}},error:function(A,i,j){if(a.isFunction(p.loaderror)){p.loaderror(A,i,j)}if(a.isFunction(f.loadError)){f.loadError(A,i,j)}A=null;z.callDownloadComplete()},beforeSend:function(j,i){if(a.isFunction(f.beforeSend)){f.beforeSend(j,i)}if(a.isFunction(p.beforesend)){p.beforesend(j,i)}}})}}else{z.callDownloadComplete();if(a.isFunction(f.loadComplete)){f.loadComplete(data)}}break}this.callBindingUpdate(s)},addForeignValues:function(c){var n=this;var t=c.datafields?c.datafields.length:0;for(var k=0;k<t;k++){var g=c.datafields[k];if(g!=undefined){if(g.values!=undefined){if(g.value==undefined){g.value=g.name}if(g.values.value==undefined){g.values.value=g.value}var r=new Array();var f,h;if(n.pageable&&n.virtualmode){f=n.pagenum*n.pagesize+1;h=(n.pagenum+1)*n.pagesize+1;if(h>n.totalrecords){h=n.totalrecords}}else{f=0;h=n.records.length}for(var l=f;l<h;l++){var m=n.records[l];var d=g.name;var s=m[g.value];if(r[s]!=undefined){m[d]=r[s]}else{for(var e=0;e<g.values.source.length;e++){var q=g.values.source[e];var b=q[g.values.value];if(b==undefined){b=q.uid}if(b==s){var o=q[g.values.name];m[d]=o;r[s]=o;break}}}}}else{if(g.value!=undefined){for(var l=0;l<n.records.length;l++){var m=n.records[l];m[g.name]=m[g.value]}}}}}},abort:function(){if(this.xhr&&this.xhr.readyState!=4){this.xhr.abort()}},_requestData:function(c,e,b){var d=this;var f=function(g){if(g.totalrecords){e.totalrecords=g.totalrecords;d.totalrecords=g.totalrecords}if(g.records){d.records=g.records;d.cachedrecords=g.records}if(a.isFunction(b.loadComplete)){b.loadComplete(data)}d.callDownloadComplete()};b.loadServerData(c,e,f)},getUniqueRecords:function(d,g){if(d&&g){var b=d.length;var l=g.length;var i=new Array();var j=new Array();for(var k=0;k<b;k++){var h=d[k];var e="";if(h==undefined){continue}for(var f=0;f<l;f++){var c=g[f];e+=h[c]+"_"}if(!j[e]){i[i.length]=h}j[e]=true}}return i},getAggregatedData:function(n,h,f){var e=f;if(!e){e=this.records}var k={};var d=e.length;if(d==0){return}if(d==undefined){return}for(var l=0;l<d;l++){var m=e[l];for(var g=0;g<n.length;g++){var c=n[g];var p=m[c.name];if(c.aggregates){k[c.name]=k[c.name]||{};var b=function(i){for(obj in i){var j=k[c.name][obj];if(j==null){k[c.name][obj]=0;j=0}if(a.isFunction(i[obj])){j=i[obj](j,p,c.name,m)}k[c.name][obj]=j}};var o=parseFloat(p);if(isNaN(o)){o=false}else{o=true}if(o){p=parseFloat(p)}if(typeof p==="number"&&isFinite(p)){a.each(c.aggregates,function(){var i=k[c.name][this];if(i==null){i=0;if(this=="min"){i=9999999999999}}if(this=="sum"||this=="avg"||this=="stdev"||this=="stdevp"||this=="var"||this=="varp"){i+=parseFloat(p)}else{if(this=="product"){if(l==0){i=parseFloat(p)}else{i*=parseFloat(p)}}else{if(this=="min"){i=Math.min(i,parseFloat(p))}else{if(this=="max"){i=Math.max(i,parseFloat(p))}else{if(this=="count"){i++}else{if(typeof(this)=="object"){b(this);return}}}}}}k[c.name][this]=i})}else{a.each(c.aggregates,function(){if(this=="min"||this=="max"||this=="count"||this=="product"||this=="sum"||this=="avg"||this=="stdev"||this=="stdevp"||this=="var"||this=="varp"){var i=k[c.name][this];if(i==null){i=0}k[c.name][this]=i;return true}if(typeof(this)=="object"){b(this)}})}}}}for(var g=0;g<n.length;g++){var c=n[g];if(k[c.name]["avg"]!=undefined){var p=k[c.name]["avg"];k[c.name]["avg"]=p/e.length}else{if(k[c.name]["count"]!=undefined){k[c.name]["count"]=d}}if(k[c.name]["stdev"]||k[c.name]["stdevp"]||k[c.name]["var"]||k[c.name]["varp"]){a.each(c.aggregates,function(v){if(this=="stdev"||this=="var"||this=="varp"||this=="stdevp"){var w=k[c.name][this];var u=d;var j=(w/d);var r=0;for(var s=0;s<d;s++){var t=e[s];var x=t[c.name];r+=(x-j)*(x-j)}var q=(this=="stdevp"||this=="varp")?u:u-1;if(q==0){q=1}if(this=="var"||this=="varp"){k[c.name][this]=r/q}else{if(this=="stdevp"||this=="stdev"){k[c.name][this]=Math.sqrt(r/q)}}}})}if(c.formatStrings){a.each(c.aggregates,function(j){var i=c.formatStrings[j];if(i){if(this=="min"||this=="max"||this=="count"||this=="product"||this=="sum"||this=="avg"||this=="stdev"||this=="stdevp"||this=="var"||this=="varp"){var q=k[c.name][this];k[c.name][this]=a.jqx.dataFormat.formatnumber(q,i,h)}else{if(typeof this=="object"){for(obj in this){var q=k[c.name][obj];k[c.name][obj]=a.jqx.dataFormat.formatnumber(q,i,h)}}}}})}}return k},bindDownloadComplete:function(c,b){this._downloadComplete[this._downloadComplete.length]={id:c,func:b}},unbindDownloadComplete:function(c){for(var b=0;b<this._downloadComplete.length;b++){if(this._downloadComplete[b].id==c){this._downloadComplete[b].func=null;this._downloadComplete.splice(b,1);break}}},callDownloadComplete:function(){for(var b=0;b<this._downloadComplete.length;b++){var c=this._downloadComplete[b];if(c.func!=null){c.func()}}},setSource:function(b){this._source=b},generatekey:function(){var b=function(){return(((1+Math.random())*65536)|0).toString(16).substring(1)};return(b()+b()+"-"+b()+"-"+b()+"-"+b()+"-"+b()+b()+b())},getGroupedRecords:function(C,F,p,x,D,v){var z=0;var u=this;var d=new Array();for(var h=0;h<C.length;h++){d[h]=u.generatekey()}if(!F){F="items"}if(!p){p="group"}if(!D){D="record"}if(!v){v="value"}var l=new Array();var f=0;var e=new Array();var k=C.length;var E=new Array();var G=this.records;var i=G.length;var y=function(q){var H=q;if(x){a.each(x,function(){if(this.name&&this.map){H[this.map]=H[this.name]}})}return H};for(var o=0;o<i;o++){var B=y(G[o]);id=B[u.uniqueId];var c=new Array();var r=0;for(h=0;h<k;h++){var j=C[h];var w=B[j];if(null==w){continue}c[r++]={value:w,hash:d[h]}}if(c.length!=k){break}var s=null;var m="";var b=-1;for(var t=0;t<c.length;t++){b++;var A=c[t].value;var g=c[t].hash;m=m+"_"+g+"_"+A;if(e[m]!=undefined&&e[m]!=null){s=e[m];continue}if(s==null){s={level:0};s[p]=A;s[D]=B;s[v]=B[v];s[F]=new Array();l[f++]=s}else{var n={parentItem:s,level:s.level+1};n[p]=A;n[F]=new Array();n[D]=B;n[v]=B[v];s[F][s[F].length]=n;s=n}e[m]=s}if(s!=null){B.parentItem=s;B.level=s.level+1;s[F][s[F].length]=B}}return l},getRecordsHierarchy:function(f,d,t,o){var b=new Array();var c=this.records;if(this.records.length==0){return null}var r=t!=null?t:"items";var l=[];var u=c;var j=u.length;var p=function(i){var v=i;if(o){a.each(o,function(){if(this.name&&this.map){v[this.map]=v[this.name]}})}return v};for(var q=0;q<j;q++){var s=a.extend({},u[q]);var n=s[d];var m=s[f];l[m]={parentid:n,item:s}}for(var q=0;q<j;q++){var s=a.extend({},u[q]);var n=s[d];var m=s[f];if(l[n]!=undefined){var s={parentid:n,item:l[m].item};var k=l[n].item;if(!k[r]){k[r]=[]}var g=k[r].length;var e=s.item;var h=p(e);k[r][g]=h;l[n].item=k;l[m]=s}else{var e=l[m].item;var h=p(e);b[b.length]=h}}return b},bindBindingUpdate:function(c,b){this._bindingUpdate[this._bindingUpdate.length]={id:c,func:b}},unbindBindingUpdate:function(c){for(var b=0;b<this._bindingUpdate.length;b++){if(this._bindingUpdate[b].id==c){this._bindingUpdate[b].func=null;this._bindingUpdate.splice(b,1);break}}},callBindingUpdate:function(b){for(var d=0;d<this._bindingUpdate.length;d++){var c=this._bindingUpdate[d];if(c.func!=null){c.func(b)}}},getid:function(e,c,d){if(a(e,c).length>0){return a(e,c).text()}if(e){if(e.toString().length>0){var b=a(c).attr(e);if(b!=null&&b.toString().length>0){return b}}}return d},loadjson:function(C,D,o){if(typeof(C)=="string"){C=a.parseJSON(C)}if(o.root==undefined){o.root=""}if(o.record==undefined){o.record=""}var C=C||D;if(!C){C=[]}var B=this;if(o.root!=""){if(C[o.root]!=undefined){C=C[o.root]}else{a.each(C,function(j){var d=this;if(this==o.root){C=this;return false}else{if(this[o.root]!=undefined){C=this[o.root]}}})}if(!C){var g=o.root.split(B.mapChar);if(g.length>0){var y=C;for(var n=0;n<g.length;n++){if(y!=undefined){y=y[g[n]]}}C=y}}}else{if(!C.length){for(obj in C){if(a.isArray(C[obj])){C=C[obj];break}}}}if(C!=null&&C.length==undefined){C=a.makeArray(C)}if(C==null||C.length==undefined){alert("JSON Parse error.");return}if(C.length==0){this.totalrecords=0;return}var f=C.length;this.totalrecords=this.virtualmode?(o.totalrecords||f):f;this.records=new Array();this.originaldata=new Array();var u=this.records;var r=!this.pageable?o.recordstartindex:this.pagesize*this.pagenum;this.recordids=new Array();if(o.loadallrecords){r=0;f=this.totalrecords}var m=0;if(this.virtualmode){r=!this.pageable?o.recordstartindex:this.pagesize*this.pagenum;m=r;r=0;f=this.totalrecords}var w=o.datafields?o.datafields.length:0;if(w==0){var b=C[0];var z=new Array();for(obj in b){var c=obj;z[z.length]={name:c}}o.datafields=z;w=z.length}var k=r;for(var t=r;t<f;t++){var e=C[t];if(e==undefined){break}if(o.record&&o.record!=""){e=e[o.record];if(e==undefined){continue}}var A=this.getid(o.id,e,t);if(typeof(A)==="object"){A=t}if(!this.recordids[A]){this.recordids[A]=e;var h={};for(var s=0;s<w;s++){var l=o.datafields[s];var q="";if(undefined==l||l==null){continue}if(l.map){var g=l.map.split(B.mapChar);if(g.length>0){var x=e;for(var n=0;n<g.length;n++){if(x!=undefined){x=x[g[n]]}}q=x}else{q=e[l.map]}if(q!=undefined&&q!=null){q=q.toString()}else{q=""}}if(q==""){q=e[l.name];if(q==undefined||q==null){q=""}if(l.value!=undefined){var v=q[l.value];if(v!=undefined){q=v}}}q=this.getvaluebytype(q,l);if(l.displayname!=undefined){h[l.displayname]=q}else{h[l.name]=q}}if(o.recordendindex<=0||r<o.recordendindex){u[m+k]=a.extend({},h);u[m+k].uid=A;this.originaldata[m+k]=a.extend({},u[t]);k++}}}this.records=u;this.cachedrecords=this.records},loadxml:function(f,x,m){if(typeof(f)=="string"){f=x=a(a.parseXML(f))}if(m.root==undefined){m.root=""}if(m.record==undefined){m.record=""}var f;if(a.jqx.browser.msie&&x){if(x.xml!=undefined){f=a(m.root+" "+m.record,a.parseXML(x.xml))}else{f=f||a(m.root+" "+m.record,x)}}else{f=f||a(m.root+" "+m.record,x)}if(!f){f=[]}var e=f.length;if(f.length==0){return}this.totalrecords=this.virtualmode?(m.totalrecords||e):e;this.records=new Array();this.originaldata=new Array();var t=this.records;var q=!this.pageable?m.recordstartindex:this.pagesize*this.pagenum;this.recordids=new Array();if(m.loadallrecords){q=0;e=this.totalrecords}var k=0;if(this.virtualmode){q=!this.pageable?m.recordstartindex:this.pagesize*this.pagenum;k=q;q=0;e=this.totalrecords}var u=m.datafields?m.datafields.length:0;if(u==0){var b=f[0];var v=new Array();for(obj in b){var c=obj;v[v.length]={name:c}}m.datafields=v;u=v.length}var l=q;for(var s=q;s<e;s++){var d=f[s];if(d==undefined){break}var w=this.getid(m.id,d,s);if(!this.recordids[w]){this.recordids[w]=d;var g={};for(var r=0;r<u;r++){var h=m.datafields[r];var o="";if(undefined==h||h==null){continue}if(h.map){o=a(h.map,d).text()}if(o==""){o=a(h.name,d).text()}var n=o;o=this.getvaluebytype(o,h);if(h.displayname!=undefined){g[h.displayname]=o}else{g[h.name]=o}}if(m.recordendindex<=0||q<m.recordendindex){t[k+l]=a.extend({},g);t[k+l].uid=w;this.originaldata[k+l]=a.extend({},t[s]);l++}}}this.records=t;this.cachedrecords=this.records},loadtext:function(u,m){if(u==null){return}var b=m.rowDelimiter||this.rowDelimiter||"\n";var g=u.split(b);var e=g.length;this.totalrecords=this.virtualmode?(m.totalrecords||e):e;this.records=new Array();this.originaldata=new Array();var r=this.records;var o=!this.pageable?m.recordstartindex:this.pagesize*this.pagenum;this.recordids=new Array();if(m.loadallrecords){o=0;e=this.totalrecords}var k=0;if(this.virtualmode){o=!this.pageable?m.recordstartindex:this.pagesize*this.pagenum;k=o;o=0;e=this.totalrecords}var s=m.datafields.length;var l=m.columnDelimiter||this.columnDelimiter;if(!l){l=(m.datatype==="tab")?"\t":","}for(var q=o;q<e;q++){var d=g[q];var t=this.getid(m.id,d,q);if(!this.recordids[t]){this.recordids[t]=d;var f={};var c=g[q].split(l);for(var p=0;p<s;p++){if(p>=c.length){continue}var h=m.datafields[p];var n=c[p];if(h.type){n=this.getvaluebytype(n,h)}var v=h.map||h.name||p.toString();f[v]=n}r[k+q]=a.extend({},f);r[k+q].uid=t;this.originaldata[k+q]=a.extend({},r[q])}}this.records=r;this.cachedrecords=this.records},getvaluebytype:function(f,c){var d=f;if(f==null){return f}if(c.type=="date"){if(f=="NaN"){f=""}else{var e=new Date(f);if(typeof f=="string"){if(c.format){var b=a.jqx.dataFormat.parsedate(f,c.format);if(b!=null){e=b}}}if(e.toString()=="NaN"||e.toString()=="Invalid Date"){if(a.jqx.dataFormat){f=a.jqx.dataFormat.tryparsedate(f)}else{f=e}}else{f=e}if(f==null){f=d}}}else{if(c.type=="float"||c.type=="number"||c.type=="decimal"){if(f=="NaN"){f=""}else{var f=parseFloat(f);if(isNaN(f)){f=d}}}else{if(c.type=="int"||c.type=="integer"){var f=parseInt(f);if(isNaN(f)){f=d}}else{if(c.type=="bool"||c.type=="boolean"){if(f!=null){if(f.toLowerCase!=undefined){if(f.toLowerCase()=="false"){f=false}else{if(f.toLowerCase()=="true"){f=true}}}}if(f==1){f=true}else{if(f==0&&f!==""){f=false}else{f=""}}}}}}return f}};a.jqx.dataFormat={};a.extend(a.jqx.dataFormat,{regexTrim:/^\s+|\s+$/g,regexInfinity:/^[+-]?infinity$/i,regexHex:/^0x[a-f0-9]+$/i,regexParseFloat:/^[+-]?\d*\.?\d*(e[+-]?\d+)?$/,toString:Object.prototype.toString,isBoolean:function(b){return typeof b==="boolean"},isObject:function(b){return(b&&(typeof b==="object"||a.isFunction(b)))||false},isDate:function(b){return b instanceof Date},arrayIndexOf:function(e,d){if(e.indexOf){return e.indexOf(d)}for(var b=0,c=e.length;b<c;b++){if(e[b]===d){return b}}return -1},isString:function(b){return typeof b==="string"},isNumber:function(b){return typeof b==="number"&&isFinite(b)},isNull:function(b){return b===null},isUndefined:function(b){return typeof b==="undefined"},isValue:function(b){return(this.isObject(b)||this.isString(b)||this.isNumber(b)||this.isBoolean(b))},isEmpty:function(b){if(!this.isString(b)&&this.isValue(b)){return false}else{if(!this.isValue(b)){return true}}b=a.trim(b).replace(/\&nbsp\;/ig,"").replace(/\&#160\;/ig,"");return b===""},startsWith:function(c,b){return c.indexOf(b)===0},endsWith:function(c,b){return c.substr(c.length-b.length)===b},trim:function(b){return(b+"").replace(this.regexTrim,"")},isArray:function(b){return this.toString.call(b)==="[object Array]"},defaultcalendar:function(){var b={"/":"/",":":":",firstDay:0,days:{names:["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],namesAbbr:["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],namesShort:["Su","Mo","Tu","We","Th","Fr","Sa"]},months:{names:["January","February","March","April","May","June","July","August","September","October","November","December",""],namesAbbr:["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec",""]},AM:["AM","am","AM"],PM:["PM","pm","PM"],eras:[{name:"A.D.",start:null,offset:0}],twoDigitYearMax:2029,patterns:{d:"M/d/yyyy",D:"dddd, MMMM dd, yyyy",t:"h:mm tt",T:"h:mm:ss tt",f:"dddd, MMMM dd, yyyy h:mm tt",F:"dddd, MMMM dd, yyyy h:mm:ss tt",M:"MMMM dd",Y:"yyyy MMMM",S:"yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",ISO:"yyyy-MM-dd hh:mm:ss",ISO2:"yyyy-MM-dd HH:mm:ss",d1:"dd.MM.yyyy",d2:"dd-MM-yyyy",zone1:"yyyy-MM-ddTHH:mm:ss-HH:mm",zone2:"yyyy-MM-ddTHH:mm:ss+HH:mm",custom:"yyyy-MM-ddTHH:mm:ss.fff"},percentsymbol:"%",currencysymbol:"$",currencysymbolposition:"before",decimalseparator:".",thousandsseparator:","};return b},expandFormat:function(f,e){e=e||"F";var d,c=f.patterns,b=e.length;if(b===1){d=c[e];if(!d){throw"Invalid date format string '"+e+"'."}e=d}else{if(b===2&&e.charAt(0)==="%"){e=e.charAt(1)}}return e},getEra:function(d,c){if(!c){return 0}if(typeof d==="string"){return 0}var g,f=d.getTime();for(var e=0,b=c.length;e<b;e++){g=c[e].start;if(g===null||f>=g){return e}}return 0},toUpper:function(b){return b.split("\u00A0").join(" ").toUpperCase()},toUpperArray:function(b){var e=[];for(var d=0,c=b.length;d<c;d++){e[d]=this.toUpper(b[d])}return e},getEraYear:function(c,e,b,f){var d=c.getFullYear();if(!f&&e.eras){d-=e.eras[b].offset}return d},getDayIndex:function(f,e,c){var b,g=f.days,d=f._upperDays;if(!d){f._upperDays=d=[this.toUpperArray(g.names),this.toUpperArray(g.namesAbbr),this.toUpperArray(g.namesShort)]}e=toUpper(e);if(c){b=this.arrayIndexOf(d[1],e);if(b===-1){b=this.arrayIndexOf(d[2],e)}}else{b=this.arrayIndexOf(d[0],e)}return b},getMonthIndex:function(j,h,d){var b=j.months,c=j.monthsGenitive||j.months,f=j._upperMonths,g=j._upperMonthsGen;if(!f){j._upperMonths=f=[this.toUpperArray(b.names),this.toUpperArray(b.namesAbbr)];j._upperMonthsGen=g=[this.toUpperArray(c.names),this.toUpperArray(c.namesAbbr)]}h=this.toUpper(h);var e=this.arrayIndexOf(d?f[1]:f[0],h);if(e<0){e=this.arrayIndexOf(d?g[1]:g[0],h)}return e},appendPreOrPostMatch:function(f,b){var e=0,h=false;for(var g=0,d=f.length;g<d;g++){var j=f.charAt(g);switch(j){case"'":if(h){b.push("'")}else{e++}h=false;break;case"\\":if(h){b.push("\\")}h=!h;break;default:b.push(j);h=false;break}}return e},getTokenRegExp:function(){return/\/|dddd|ddd|dd|d|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f|zzz|zz|z|gg|g/g},formatlink:function(b,d){var c="";if(d&&d.target){c="target="+d.target}if(c!=""){return"<a "+c+' href="'+b+'">'+b+"</a>"}return'<a href="'+b+'">'+b+"</a>"},formatemail:function(b){return'<a href="mailto:'+b+'">'+b+"</a>"},formatnumber:function(p,o,k){if(k==undefined||k==null||k==""){k=this.defaultcalendar()}if(!this.isNumber(p)){p*=1}var l;if(o.length>1){l=parseInt(o.slice(1),10)}var r={};var m=o.charAt(0).toUpperCase();r.thousandsSeparator=k.thousandsseparator;r.decimalSeparator=k.decimalseparator;switch(m){case"D":case"d":case"F":case"f":r.decimalPlaces=l;break;case"N":case"n":r.decimalPlaces=0;break;case"C":case"c":r.decimalPlaces=l;if(k.currencysymbolposition=="before"){r.prefix=k.currencysymbol}else{r.suffix=k.currencysymbol}break;case"P":case"p":r.suffix=k.percentsymbol;r.decimalPlaces=l;break;default:throw"Bad number format specifier: "+m}if(this.isNumber(p)){var f=(p<0);var d=p+"";var n=(r.decimalSeparator)?r.decimalSeparator:".";var b;if(this.isNumber(r.decimalPlaces)){var g=r.decimalPlaces;var j=Math.pow(10,g);d=Math.round(p*j)/j+"";b=d.lastIndexOf(".");if(g>0){if(b<0){d+=n;b=d.length-1}else{if(n!=="."){d=d.replace(".",n)}}while((d.length-1-b)<g){d+="0"}}}if(r.thousandsSeparator){var q=r.thousandsSeparator;b=d.lastIndexOf(n);b=(b>-1)?b:d.length;var e=d.substring(b);var c=-1;for(var h=b;h>0;h--){c++;if((c%3===0)&&(h!==b)&&(!f||(h>1))){e=q+e}e=d.charAt(h-1)+e}d=e}d=(r.prefix)?r.prefix+d:d;d=(r.suffix)?d+r.suffix:d;return d}else{return p}},tryparsedate:function(p,h){if(h==undefined||h==null){h=this.defaultcalendar()}var l=this;if(p==""){return null}if(p!=null&&!p.substring){p=p.toString()}if(p!=null&&p.substring(0,6)=="/Date("){var n=/^\/Date\((-?\d+)(\+|-)?(\d+)?\)\/$/;var e=new Date(+p.replace(/\/Date\((\d+)\)\//,"$1"));if(e=="Invalid Date"){var f=p.match(/^\/Date\((\d+)([-+]\d\d)(\d\d)\)\/$/);var e=null;if(f){e=new Date(1*f[1]+3600000*f[2]+60000*f[3])}}if(e==null||e=="Invalid Date"||isNaN(e)){var i=n.exec(p);if(i){var q=new Date(parseInt(i[1]));if(i[2]){var b=parseInt(i[3]);if(i[2]==="-"){b=-b}var k=q.getUTCMinutes();q.setUTCMinutes(k-b)}if(!isNaN(q.valueOf())){return q}}}return e}patterns=h.patterns;for(prop in patterns){e=l.parsedate(p,patterns[prop],h);if(e){return e}}if(p!=null){var d=null;var o=[":","/","-"];var j=true;for(var c=0;c<o.length;c++){if(p.indexOf(o[c])!=-1){j=false}}if(j){var g=new Number(p);if(!isNaN(g)){return new Date(g)}}}return null},getparseregexp:function(b,n){var p=b._parseRegExp;if(!p){b._parseRegExp=p={}}else{var f=p[n];if(f){return f}}var l=this.expandFormat(b,n).replace(/([\^\$\.\*\+\?\|\[\]\(\)\{\}])/g,"\\\\$1"),j=["^"],c=[],i=0,e=0,s=this.getTokenRegExp(),g;while((g=s.exec(l))!==null){var r=l.slice(i,g.index);i=s.lastIndex;e+=this.appendPreOrPostMatch(r,j);if(e%2){j.push(g[0]);continue}var d=g[0],h=d.length,o;switch(d){case"dddd":case"ddd":case"MMMM":case"MMM":case"gg":case"g":o="(\\D+)";break;case"tt":case"t":o="(\\D*)";break;case"yyyy":case"fff":case"ff":case"f":o="(\\d{"+h+"})";break;case"dd":case"d":case"MM":case"M":case"yy":case"y":case"HH":case"H":case"hh":case"h":case"mm":case"m":case"ss":case"s":o="(\\d\\d?)";break;case"zzz":o="([+-]?\\d\\d?:\\d{2})";break;case"zz":case"z":o="([+-]?\\d\\d?)";break;case"/":o="(\\"+b["/"]+")";break;default:throw"Invalid date format pattern '"+d+"'.";break}if(o){j.push(o)}c.push(g[0])}this.appendPreOrPostMatch(l.slice(i),j);j.push("$");var q=j.join("").replace(/\s+/g,"\\s+"),k={regExp:q,groups:c};return p[n]=k},outOfRange:function(d,b,c){return d<b||d>c},expandYear:function(g,e){var c=new Date(),b=getEra(c);if(e<100){var d=g.twoDigitYearMax;d=typeof d==="string"?new Date().getFullYear()%100+parseInt(d,10):d;var f=this.getEraYear(c,g,b);e+=f-(f%100);if(e>d){e-=100}}return e},parsedate:function(z,G,u){if(u==undefined||u==null){u=this.defaultcalendar()}z=this.trim(z);var r=u,L=this.getparseregexp(r,G),k=new RegExp(L.regExp).exec(z);if(k===null){return null}var H=L.groups,x=null,p=null,K=null,J=null,q=null,g=0,C,B=0,I=0,b=0,d=null,s=false;for(var D=0,F=H.length;D<F;D++){var c=k[D+1];if(c){var y=H[D],f=y.length,h=parseInt(c,10);switch(y){case"dd":case"d":J=h;if(this.outOfRange(J,1,31)){return null}break;case"MMM":case"MMMM":K=this.getMonthIndex(r,c,f===3);if(this.outOfRange(K,0,11)){return null}break;case"M":case"MM":K=h-1;if(this.outOfRange(K,0,11)){return null}break;case"y":case"yy":case"yyyy":p=f<4?this.expandYear(r,h):h;if(this.outOfRange(p,0,9999)){return null}break;case"h":case"hh":g=h;if(g===12){g=0}if(this.outOfRange(g,0,11)){return null}break;case"H":case"HH":g=h;if(this.outOfRange(g,0,23)){return null}break;case"m":case"mm":B=h;if(this.outOfRange(B,0,59)){return null}break;case"s":case"ss":I=h;if(this.outOfRange(I,0,59)){return null}break;case"tt":case"t":s=r.PM&&(c===r.PM[0]||c===r.PM[1]||c===r.PM[2]);if(!s&&(!r.AM||(c!==r.AM[0]&&c!==r.AM[1]&&c!==r.AM[2]))){return null}break;case"f":case"ff":case"fff":b=h*Math.pow(10,3-f);if(this.outOfRange(b,0,999)){return null}break;case"ddd":case"dddd":q=this.getDayIndex(r,c,f===3);if(this.outOfRange(q,0,6)){return null}break;case"zzz":var e=c.split(/:/);if(e.length!==2){return null}C=parseInt(e[0],10);if(this.outOfRange(C,-12,13)){return null}var n=parseInt(e[1],10);if(this.outOfRange(n,0,59)){return null}d=(C*60)+(startsWith(c,"-")?-n:n);break;case"z":case"zz":C=h;if(this.outOfRange(C,-12,13)){return null}d=C*60;break;case"g":case"gg":var t=c;if(!t||!r.eras){return null}t=trim(t.toLowerCase());for(var E=0,A=r.eras.length;E<A;E++){if(t===r.eras[E].name.toLowerCase()){x=E;break}}if(x===null){return null}break}}}var o=new Date(),w,m=r.convert;w=o.getFullYear();if(p===null){p=w}else{if(r.eras){p+=r.eras[(x||0)].offset}}if(K===null){K=0}if(J===null){J=1}if(m){o=m.toGregorian(p,K,J);if(o===null){return null}}else{o.setFullYear(p,K,J);if(o.getDate()!==J){return null}if(q!==null&&o.getDay()!==q){return null}}if(s&&g<12){g+=12}o.setHours(g,B,I,b);if(d!==null){var v=o.getMinutes()-(d+o.getTimezoneOffset());o.setHours(o.getHours()+parseInt(v/60,10),v%60)}return o},cleardatescache:function(){this.datescache=new Array()},formatdate:function(u,y,p){if(p==undefined||p==null){p=this.defaultcalendar()}if(typeof u==="string"){return u}var e=u.toString()+"_"+y;if(this.datescache&&this.datescache[e]){return this.datescache[e]}if(!y||!y.length||y==="i"){var A;A=this.formatDate(u,p.patterns.F,culture);return A}var v=p.eras,c=y==="s";y=this.expandFormat(p,y);A=[];var h,w=["0","00","000"],l,m,b=/([^d]|^)(d|dd)([^d]|$)/g,z=0,r=this.getTokenRegExp(),d;function j(B,E){var D,C=B+"";if(E>1&&C.length<E){D=(w[E-2]+C);return D.substr(D.length-E,E)}else{D=C}return D}function x(){if(l||m){return l}l=b.test(y);m=true;return l}function f(C,B){if(d){return d[B]}if(C.getMonth!=undefined){switch(B){case 0:return C.getFullYear();case 1:return C.getMonth();case 2:return C.getDate()}}}for(;;){var i=r.lastIndex,q=r.exec(y);var n=y.slice(i,q?q.index:y.length);z+=this.appendPreOrPostMatch(n,A);if(!q){break}if(z%2){A.push(q[0]);continue}var s=q[0],g=s.length;switch(s){case"ddd":case"dddd":var o=(g===3)?p.days.namesAbbr:p.days.names;A.push(o[u.getDay()]);break;case"d":case"dd":l=true;A.push(j(f(u,2),g));break;case"MMM":case"MMMM":var t=f(u,1);A.push(p.months[g===3?"namesAbbr":"names"][t]);break;case"M":case"MM":A.push(j(f(u,1)+1,g));break;case"y":case"yy":case"yyyy":t=this.getEraYear(u,p,this.getEra(u,v),c);if(g<4){t=t%100}A.push(j(t,g));break;case"h":case"hh":h=u.getHours()%12;if(h===0){h=12}A.push(j(h,g));break;case"H":case"HH":A.push(j(u.getHours(),g));break;case"m":case"mm":A.push(j(u.getMinutes(),g));break;case"s":case"ss":A.push(j(u.getSeconds(),g));break;case"t":case"tt":t=u.getHours()<12?(p.AM?p.AM[0]:" "):(p.PM?p.PM[0]:" ");A.push(g===1?t.charAt(0):t);break;case"f":case"ff":case"fff":A.push(j(u.getMilliseconds(),3).substr(0,g));break;case"z":case"zz":h=u.getTimezoneOffset()/60;A.push((h<=0?"+":"-")+j(Math.floor(Math.abs(h)),g));break;case"zzz":h=u.getTimezoneOffset()/60;A.push((h<=0?"+":"-")+j(Math.floor(Math.abs(h)),2)+":"+j(Math.abs(u.getTimezoneOffset()%60),2));break;case"g":case"gg":if(p.eras){A.push(p.eras[getEra(u,v)].name)}break;case"/":A.push(p["/"]);break;default:throw"Invalid date format pattern '"+s+"'.";break}}var k=A.join("");if(!this.datescache){this.datescache=new Array()}this.datescache[e]=k;return k}})})(jQuery);