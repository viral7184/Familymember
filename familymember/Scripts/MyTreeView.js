$(document).ready(function){
    $(".collapsing").live("click",function(e)){
    e.preventDefault();

        var this1=$(this);
        var data={
            pid: $(this).attr('pid')
        };
     
        var isLoaded=$(this1).attr('data-loading');
        if(isLoaded == "false")
        {
            $(this1).addClass("loadingp");
            $(this1).removeClass("collapse");

            $.ajax({
                url:"/Home/Getsubmenu",
                type:"GET",
                data:data,
                dataType:"json",
                success:function(d){
                    $(this1).removeClass("loadingp");

                    if(d.length > 0)
                    {
                        var $ul = $("<ul></ul>");
                        $.each(d, function(i,ele){
                            $ul.append(
                                $("<li></li>").append(
                                 "<span class='collapse collapsible' data-loaded='false' pid='"+ele.FIRSTNAME+"'>&nbsp;</span>" +
                                 "<span><a href='"+ele.FIRSTNAME+"'>"+ele.FIRSTNAME+"</a></span>"
                                )
                                )
                        });
                        $(this1).parent().append($ul);
                        $(this1).addClass('collapse');
                        $(this1).toggleClass('collapse');
                        $(this1).closest('li').children('ul').slideDown;
                    }
                    else{
                        $(this1).css({'display':'inline-block','width':'15px'});
                    }
                    $(this1).attr('data-loaded',true);
                },
                error:function(){
                    alert("Error!");
                }
            });
        }
        else{
            $(this1).toggleClass("collapse expand");
            $(this1).closest('li').children('ul').slideDown();
        }
    });
});