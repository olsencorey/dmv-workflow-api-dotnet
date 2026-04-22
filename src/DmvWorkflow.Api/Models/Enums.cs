namespace DmvWorkflow.Api.Models;

public enum ChannelType { Web, Kiosk, Clerk }
public enum SessionStatus { Started, VehicleMatched, Quoted, PaymentAuthorized, Completed, Cancelled }
public enum PaymentMethodType { Card, Cash, Waived }
public enum DeliveryMethodType { PrintAtKiosk, Mail, DigitalReceipt }
