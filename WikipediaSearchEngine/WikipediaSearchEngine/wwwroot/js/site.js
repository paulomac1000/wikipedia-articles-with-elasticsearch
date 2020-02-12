// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#elastic-search-input").click(function () {
    refreshList();
});

$("#elastic-search-input").keydown(function () {
    refreshList();
});

function refreshList() {
    setTimeout(function () {
        var url = ("/api/simple-query-string?term=" + $("#elastic-search-input").val());
        $.get(url, function (data, status) {
            updateData(data);
        });
    }, 1);
}

function updateData(data) {
    var template =
        '<li>' +
            '<h3 class="title"></h3>' +
            '<p class="content"></p>' +
            '<p class="timestamp"></p>' +
        '</li>';

    var options = {
        valueNames: ['title', 'content', 'timestamp'],
        item: template
    };

    $("#elastic-result-list").empty();
    var elasticList = new List('elastic-result-list-container', options, data);
}