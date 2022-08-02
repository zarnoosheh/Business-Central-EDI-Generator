
pageextension 50101 SalesOrderListExt extends "Sales Order List"
{
    actions
    {
        addfirst(processing)
        {
            group("EDI")
            {
                Caption = 'EDI';
                Image = SendTo;

                group("Receive/850")
                {
                    Caption = 'Receive/850';
                    Image = SendTo;

                    action("Amazon")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        begin
                            Hyperlink('BusinessCentralEDI: 850');
                        end;
                    }
                    action("Walmart")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        begin
                            Message('Receive EDI 850 from WALMART.');
                        end;
                    }
                    action("Costco")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        begin
                            Message('Receive EDI 850 from COSTCO.');
                        end;
                    }
                }

                group("SEND")
                {
                    Caption = 'Send';
                    Image = SendTo;

                    action("Invoice/810")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        var
                            Parm: Text;
                        begin
                            Parm := format(Rec."No.");
                            // MESSAGE(Parm);
                            Hyperlink('BusinessCentralEDI: 810|' + Parm);
                        end;
                    }
                    action("ASN/856")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        begin
                            Message('All pending ASNs/856s were sent to the associated customers.');
                        end;
                    }
                    action("OrderConfo/852")
                    {
                        Promoted = false;
                        ApplicationArea = All;
                        trigger OnAction()
                        begin
                            Message('All pending order confirmations were sent to the associated customers. ');
                        end;
                    }
                }

            }

        }
    }
}