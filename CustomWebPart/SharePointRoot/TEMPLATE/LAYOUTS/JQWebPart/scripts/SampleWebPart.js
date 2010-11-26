var ViewSample = function () {
    this.gridViewSample = null;
    this.init();
};

ViewSample.prototype = {
    init: function () {
        var self = this;
        self.initViewSample();
    },
    initViewSample: function () {
        var colModel = [
            { name: 'ID', index: 'ID', hidden: true, search: false },
            { name: 'ModifiedByID', index: 'ModifiedByID', hidden: true, search: false },
            { name: 'Name', index: 'Name', width: 260, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn']} },
            { name: 'EmailAddress', index: 'EmailAddress', width: 200, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn'] }, search: false },
            { name: 'Phone', index: 'Phone', width: 150, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn'] }, search: false },
            { name: 'Notes', index: 'Notes', width: 230, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn'] }, search: false },
            { name: 'OrdersHistory', index: 'OrdersHistory', width: 270, sortable: false, search: false },
            { name: 'Registered', index: 'Registered', width: 250, sortable: true, search: false, search: false },
            { name: 'Modified', index: 'Modified', width: 250, sortable: true, search: false, search: false },
            { name: 'ModifiedBy', index: 'ModifiedBy', width: 250, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn'] }, search: false }
            ];

        var colNames = ['ID', 'ModifiedByID', 'Name', 'Email', 'Phone', 'Notes', 'Orders history', 'Registered', 'Modified', 'Modified By'];

        /*these should come from the code behind
        alert(varDetailsControlContainerId);
        alert(varJQGridPagerID);
        alert(varPageUrl);*/
        
        this.gridViewSample = jQuery(".SAMPLE_WP_LIST").jqGrid({
            url: document.location.protocol + "//" + document.location.host + "/_layouts/JQWebPart/GetSampleGridContent.ashx",
            datatype: "json",
            colNames: colNames,
            colModel: colModel,
            sortname: 'Name',
            viewrecords: true,
            sortorder: "desc",
            caption: "Sample Web Part",
            width: 1050,
            height: 400,
            rowNum: 10,
            rowList: [10, 20, 30, 40],
            loadtext: 'Loading records..',
            pager: $(".SAMPLE_WP_PAGER"),
            rownumbers: true,
            gridview: true,
            cellEdit: false,
            mtype: "POST",
            gridComplete: function () {
                var ids = jQuery(".SAMPLE_WP_LIST").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var rowData = jQuery('.SAMPLE_WP_LIST').getRowData(ids[i]);
                    var linkOrders = '<a onclick="displayUserControl(\'' + varDetailsControlContainerId  +'\', \'' + rowData.ID + '\');">' + rowData.OrdersHistory + '</a>';
                    var href = document.location.protocol + "//" + document.location.host + "/_layouts/userdisp.aspx?ID=" + rowData.ModifiedByID + "&Source=" + varPageUrl;
                    var linkAprrovedBy = '<a href="' + href + '">' + rowData.ModifiedBy + '</a>';

                    jQuery(".SAMPLE_WP_LIST").jqGrid('setRowData', ids[i], { OrdersHistory: linkOrders, ModifiedBy: linkAprrovedBy });
                }
            },
            jsonReader: {
                root: "Rows",
                page: "Page",
                total: "Total",
                records: "Records",
                repeatitems: false,
                userdata: "UserData",
                id: "ID"
            }
        });
        this.gridViewSample.jqGrid('navGrid', varJQGridPagerID, { edit: false, add: false, del: false, search: true });

    }
}
$(document).ready(function() {
    var viewSample = new ViewSample();
});

