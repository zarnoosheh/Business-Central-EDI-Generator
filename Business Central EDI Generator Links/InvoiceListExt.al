
pageextension 50100 InvoiceListExt extends "Posted Sales Invoices"
{
    actions
    {
        addlast(processing)
        {
            action("Send To Amazon Vendor")
            {
                Promoted = true;
                PromotedCategory = Process;
                Image = SendTo;
                ApplicationArea = All;
                trigger OnAction()
                begin
                    Message('All data transmitted over EDI to Amazon Vendor.');
                end;
            }
        }
    }
}