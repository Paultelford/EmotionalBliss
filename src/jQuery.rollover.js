/**
 * This plugin can be used in 4 ways, this is only 1. Make sure to check
 * the other 3 links as well to see all the Modes.
 */
function wireEvents()
{
    jQuery(function( $ ){			
        //Function is sometimes firing twice within atlas updatepanels causing *.jp to be rendered as *22.jpg
        //So remove the 2 from all existing links to be on the safe side
        /*
	    $.preload( 'input.rollover', {
	        find:'.jpg',
	        replace:'2.jpg'
	    });
	    $.preload( 'img.rollover', {
	        find:'.jpg',
	        replace:'2.jpg'
	    }); */
	    //Now preload
	    $.preload( 'input.rollover', {
	        find:'.jpg',
	        replace:'2.jpg'
	    });
	    $.preload( 'img.rollover', {
	        find:'.jpg',
	        replace:'2.jpg'
	    });
	    /*		or
	    $('#rollover-images img').preload({
		    find:'.jpg',
	        replace:'_over.jpg'
	    });
	    */
	    //add animation to <input> tags (for asp:ImageButton) and <img> tags.
	    $('input.rollover').hover(function(){
		    if(this.src.indexOf("2.jpg")==-1)this.src = this.src.replace('.jpg','2.jpg');	
	    },function(){
		    this.src = this.src.replace('2','');
	    });
	    $('img.rollover').hover(function(){
		    if(this.src.indexOf("2.jpg")==-1)this.src = this.src.replace('.jpg','2.jpg');	
	    },function(){
		    this.src = this.src.replace('2','');
	    });
    });
}
