// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const updateResults = () => {

    $.ajax({
        url:"/home/sortresults",
        dataType:"html",
        type:"post",
        data: {
            svm : "boom",
            sortElement : "Artist",
            sortDirection : "ASC"
        },
        success: (o)=> {
            alert(o);
        },
        error: (e) => {
            alert(e);
        }
    })
}