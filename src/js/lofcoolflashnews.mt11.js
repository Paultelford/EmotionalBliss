/**
 * @version		$Id:  $Revision
 * @package		mootool
 * @subpackage	lofflashnews
 * @copyright	Copyright (C) JAN 2010 LandOfCoder.com <@emai:landofcoder@gmail.com>. All rights reserved.
 * @website http://landofcoder.com/opensource/mootool/77-the-breakingnews-plug-in.html
 * @license		This plugin is dual-licensed under the GNU General Public License and the MIT License 
 */


if( typeof(LofSlideshow) == 'undefined' ){
	var LofFlashContent = new Class( {
		initialize:function( eMain, eNavigator, options ){
			this.setting = Object.extend({
				enableNavigator : false,
				navigator		: null,
				autoStart		: true,
				descStyle	    : 'sliding',
				mainItemClass		: '.lof-main-item',
				navigatorItemClass  : '.lof-navigator-item' ,
				navigatorEvent		: 'click',
				interval	  	 	:2000,
				auto:true
			}, options || {} );
			
			this.currentNo  = 0;
			this.nextNo     = null;
			this.previousNo = null;
			this.totalItems = 0;
			this.fxItems	= [];	
			this.minSize 	= 0;
		
			if( $defined(eMain) ){
				this.slides = eMain.getElements( this.setting.mainItemClass );
				this.maxWidth  = eMain.getStyle('width').toInt();
				this.maxHeight = eMain.getStyle('height').toInt();
				this.totalItems = this.slides.length;
				 styleMode = this.__getStyleMode(); 
				this.styleMode=styleMode;
				var fx =  Object.extend({waiting:false, onComplete:this.onComplete.bind(this)}, this.setting.fxObject );
				this.slides.each( function(item, index) {
					item.setStyles( eval('({"'+styleMode[0]+'": index * this.maxSize,"'+styleMode[1]+'":Math.abs(this.maxSize),"display" : "block"})') );		
					this.fxItems[index] = new Fx.Styles( item,  fx );
				}.bind(this) );
				if( styleMode[0] == 'opacity' || styleMode[0] =='z-index' ){
					this.slides[0].setStyle(styleMode[0],'1');
				}
				eMain.addEvents( { 'mouseenter' : this.stop.bind(this),
							   'mouseleave' :function(e){ 
									this.play( this.setting.interval,'next', true );
								}.bind(this) } );
			}
			if( $defined(eNavigator) ){
				this.navigatorItems = eNavigator.getElements( this.setting.navigatorItemClass );
				this.navigatorItems.each( function(item,index) {
					item.addEvent( this.setting.navigatorEvent, function(){		 
						this.jumping( index, true );
						this.setNavActive( index, item );	
							
					}.bind(this) );
				}.bind(this) );
				this.setNavActive( 0 );
			}
		},
		setNavActive:function( index, item ){
			if( $defined(this.navigatorItems) ){ 
				this.navigatorItems.removeClass('active');
				this.navigatorItems[index].addClass('active');	
			}
		},
		__getStyleMode:function(){
			switch( this.setting.layoutStyle ){
				case 'opacity': this.maxSize=0; this.minSize=1; return ['opacity','opacity'];
				case 'vrup':    this.maxSize=this.maxHeight;    return ['top','height'];
				case 'vrdown':  this.maxSize=-this.maxHeight;   return ['top','height'];
				case 'hrright': this.maxSize=-this.maxWidth;    return ['left','width'];
				case 'hrleft':
				default: this.maxSize=this.maxWidth; return ['left','width'];
			}
		},
		registerButtonsControl:function( eventHandler, objects ){
			if( $defined(objects) && this.totalItems > 1 ){
				for( var action in objects ){
					if( $defined(this[action.toString()])  && $defined(objects[action]) ){
						objects[action].addEvent( eventHandler, this[action.toString()].bind(this, [true]) );
					}
				}
			}
			return this;	
		},
		start:function( isStart, obj ){
			this.setting.auto = isStart;
			// if use the preload image.
			if( obj ) {
				var images = [] 
				this.slides.getElements('img').each( function(item, index){
					images[index] = item.getProperty('src');
				
				} );
				var loader = new Asset.images(images, { onComplete:function(){	
					(function(){																
					new Fx.Style( obj ,'opacity',{ transition:Fx.Transitions.Quad.easeInOut,
														 duration:800} ).start(1,0)}).delay(400);		
					
					if( isStart && this.totalItems > 0 ){this.play( this.setting.interval,'next', true );}	
				}.bind(this) } ); 
			} else {
				if( isStart && this.totalItems > 0 ){this.play( this.setting.interval,'next', true );}	
			}
		},
		onComplete:function(obj){
		},	
		onProcessing:function( item, manual, start, end ){			
			this.previousNo = this.currentNo + (this.currentNo>0 ? -1 : this.totalItems-1);
			this.nextNo 	= this.currentNo + (this.currentNo < this.totalItems-1 ? 1 : 1- this.totalItems);	
			if( manual ) { this.stop(); }
			return this;
		},
		finishFx:function( manual ){
			if( manual ) this.stop();
			if( manual && this.setting.auto )
				this.play( this.setting.interval,'next', true );
			
			this.setNavActive( this.currentNo );	
		},
		getObjectDirection:function( start, end ){
			return eval("({'"+this.styleMode[0]+"':["+start+", "+end+"]})");	
		},
		fxStart:function( index, obj ){
			this.fxItems[index].stop().start( obj );
			return this;
		},
		jumping:function( no, manual ){
			
			if( this.currentNo == no ) return;		
			this.onProcessing( null, manual, 0, this.maxSize )
				.fxStart( no, this.getObjectDirection(this.maxSize , this.minSize) )
				.fxStart( this.currentNo, this.getObjectDirection(this.minSize,  -this.maxSize) )
				.finishFx( manual );	
					
				this.currentNo  = no;
		},
		next:function( manual, jump ){
			this.currentNo += (this.currentNo < this.totalItems-1) ? 1 : (1 - this.totalItems);	
			this.onProcessing( null, manual, 0, this.maxSize )
				.fxStart( this.currentNo, this.getObjectDirection(this.maxSize ,this.minSize) )
				.fxStart( this.previousNo, this.getObjectDirection(this.minSize, -this.maxSize) )
				.finishFx( manual );
		},
		previous:function( manual ){
			this.currentNo += this.currentNo > 0 ? -1 : this.totalItems - 1;
			this.onProcessing( null, manual, -this.maxWidth, this.minSize )
				.fxStart( this.nextNo, this.getObjectDirection(this.minSize, this.maxSize) )
				.fxStart( this.currentNo,  this.getObjectDirection(-this.maxSize, this.minSize) )
				.finishFx( manual	);			
		},
		stop:function(){ $clear(this.isRun ); },
		play:function( delay, direction, wait ){
			this.stop(); 
			if(!wait){ this[direction](false); }
			this.isRun = this[direction].periodical(delay,this,false);
		}
	} );
}
/**
 * Allow display the tooltip when the mouse hover a button in the list.
 */
window.addEvent('domready', function(){
        //do your tips stuff in here...
	if( typeof LofZoomTip != Object ){		
	   var LofZoomTip = new Tips($$('.hasLofTip'), {
		  className: 'tip', //this is the prefix for the CSS class
		  offsets: {
					'x': -81,       //default is 16
					'y': -130        //default is 16
		  },
		  fixed:true,
		  initialize:function(){
			  this.fx = new Fx.Style(this.toolTip, 'opacity', 
							 { duration:500, 
							   wait: false}).set(0);
		  },
		  onShow: function(toolTip) {
			 this.fx.start(1,1);
		  },
		  onHide: function(toolTip) {
			  this.fx.start( 0, 0 );
		  }
	   });
	}
});
