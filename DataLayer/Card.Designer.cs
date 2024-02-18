﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("CardModel", "FK__CARDS__STATUSID__286302EC", "STATUS", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(DataLayer.STATUS), "CARDS", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(DataLayer.CARDS), true)]
[assembly: EdmRelationshipAttribute("CardModel", "FK__CARDS__USERID__29572725", "USERS", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(DataLayer.USERS), "CARDS", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(DataLayer.CARDS), true)]

#endregion

namespace DataLayer
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class CardEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new CardEntities object using the connection string found in the 'CardEntities' section of the application configuration file.
        /// </summary>
        public CardEntities() : base("name=CardEntities", "CardEntities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new CardEntities object.
        /// </summary>
        public CardEntities(string connectionString) : base(connectionString, "CardEntities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new CardEntities object.
        /// </summary>
        public CardEntities(EntityConnection connection) : base(connection, "CardEntities")
        {
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<CARDS> CARDS
        {
            get
            {
                if ((_CARDS == null))
                {
                    _CARDS = base.CreateObjectSet<CARDS>("CARDS");
                }
                return _CARDS;
            }
        }
        private ObjectSet<CARDS> _CARDS;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<STATUS> STATUS
        {
            get
            {
                if ((_STATUS == null))
                {
                    _STATUS = base.CreateObjectSet<STATUS>("STATUS");
                }
                return _STATUS;
            }
        }
        private ObjectSet<STATUS> _STATUS;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<USERS> USERS
        {
            get
            {
                if ((_USERS == null))
                {
                    _USERS = base.CreateObjectSet<USERS>("USERS");
                }
                return _USERS;
            }
        }
        private ObjectSet<USERS> _USERS;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the CARDS EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCARDS(CARDS cARDS)
        {
            base.AddObject("CARDS", cARDS);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the STATUS EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToSTATUS(STATUS sTATUS)
        {
            base.AddObject("STATUS", sTATUS);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the USERS EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToUSERS(USERS uSERS)
        {
            base.AddObject("USERS", uSERS);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="CardModel", Name="CARDS")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CARDS : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new CARDS object.
        /// </summary>
        /// <param name="cARDID">Initial value of the CARDID property.</param>
        /// <param name="nAME">Initial value of the NAME property.</param>
        public static CARDS CreateCARDS(global::System.Int32 cARDID, global::System.String nAME)
        {
            CARDS cARDS = new CARDS();
            cARDS.CARDID = cARDID;
            cARDS.NAME = nAME;
            return cARDS;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 CARDID
        {
            get
            {
                return _CARDID;
            }
            set
            {
                if (_CARDID != value)
                {
                    OnCARDIDChanging(value);
                    ReportPropertyChanging("CARDID");
                    _CARDID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("CARDID");
                    OnCARDIDChanged();
                }
            }
        }
        private global::System.Int32 _CARDID;
        partial void OnCARDIDChanging(global::System.Int32 value);
        partial void OnCARDIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String NAME
        {
            get
            {
                return _NAME;
            }
            set
            {
                OnNAMEChanging(value);
                ReportPropertyChanging("NAME");
                _NAME = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("NAME");
                OnNAMEChanged();
            }
        }
        private global::System.String _NAME;
        partial void OnNAMEChanging(global::System.String value);
        partial void OnNAMEChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }
            set
            {
                OnDESCRIPTIONChanging(value);
                ReportPropertyChanging("DESCRIPTION");
                _DESCRIPTION = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("DESCRIPTION");
                OnDESCRIPTIONChanged();
            }
        }
        private global::System.String _DESCRIPTION;
        partial void OnDESCRIPTIONChanging(global::System.String value);
        partial void OnDESCRIPTIONChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String COLOR
        {
            get
            {
                return _COLOR;
            }
            set
            {
                OnCOLORChanging(value);
                ReportPropertyChanging("COLOR");
                _COLOR = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("COLOR");
                OnCOLORChanged();
            }
        }
        private global::System.String _COLOR;
        partial void OnCOLORChanging(global::System.String value);
        partial void OnCOLORChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> STATUSID
        {
            get
            {
                return _STATUSID;
            }
            set
            {
                OnSTATUSIDChanging(value);
                ReportPropertyChanging("STATUSID");
                _STATUSID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("STATUSID");
                OnSTATUSIDChanged();
            }
        }
        private Nullable<global::System.Int32> _STATUSID;
        partial void OnSTATUSIDChanging(Nullable<global::System.Int32> value);
        partial void OnSTATUSIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> USERID
        {
            get
            {
                return _USERID;
            }
            set
            {
                OnUSERIDChanging(value);
                ReportPropertyChanging("USERID");
                _USERID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("USERID");
                OnUSERIDChanged();
            }
        }
        private Nullable<global::System.Int32> _USERID;
        partial void OnUSERIDChanging(Nullable<global::System.Int32> value);
        partial void OnUSERIDChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("CardModel", "FK__CARDS__STATUSID__286302EC", "STATUS")]
        public STATUS STATUS
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<STATUS>("CardModel.FK__CARDS__STATUSID__286302EC", "STATUS").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<STATUS>("CardModel.FK__CARDS__STATUSID__286302EC", "STATUS").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<STATUS> STATUSReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<STATUS>("CardModel.FK__CARDS__STATUSID__286302EC", "STATUS");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<STATUS>("CardModel.FK__CARDS__STATUSID__286302EC", "STATUS", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("CardModel", "FK__CARDS__USERID__29572725", "USERS")]
        public USERS USERS
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<USERS>("CardModel.FK__CARDS__USERID__29572725", "USERS").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<USERS>("CardModel.FK__CARDS__USERID__29572725", "USERS").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<USERS> USERSReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<USERS>("CardModel.FK__CARDS__USERID__29572725", "USERS");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<USERS>("CardModel.FK__CARDS__USERID__29572725", "USERS", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="CardModel", Name="STATUS")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class STATUS : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new STATUS object.
        /// </summary>
        /// <param name="sTATUSID">Initial value of the STATUSID property.</param>
        public static STATUS CreateSTATUS(global::System.Int32 sTATUSID)
        {
            STATUS sTATUS = new STATUS();
            sTATUS.STATUSID = sTATUSID;
            return sTATUS;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 STATUSID
        {
            get
            {
                return _STATUSID;
            }
            set
            {
                if (_STATUSID != value)
                {
                    OnSTATUSIDChanging(value);
                    ReportPropertyChanging("STATUSID");
                    _STATUSID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("STATUSID");
                    OnSTATUSIDChanged();
                }
            }
        }
        private global::System.Int32 _STATUSID;
        partial void OnSTATUSIDChanging(global::System.Int32 value);
        partial void OnSTATUSIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String STATUSNAME
        {
            get
            {
                return _STATUSNAME;
            }
            set
            {
                OnSTATUSNAMEChanging(value);
                ReportPropertyChanging("STATUSNAME");
                _STATUSNAME = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("STATUSNAME");
                OnSTATUSNAMEChanged();
            }
        }
        private global::System.String _STATUSNAME;
        partial void OnSTATUSNAMEChanging(global::System.String value);
        partial void OnSTATUSNAMEChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("CardModel", "FK__CARDS__STATUSID__286302EC", "CARDS")]
        public EntityCollection<CARDS> CARDS
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<CARDS>("CardModel.FK__CARDS__STATUSID__286302EC", "CARDS");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<CARDS>("CardModel.FK__CARDS__STATUSID__286302EC", "CARDS", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="CardModel", Name="USERS")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class USERS : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new USERS object.
        /// </summary>
        /// <param name="uSERID">Initial value of the USERID property.</param>
        public static USERS CreateUSERS(global::System.Int32 uSERID)
        {
            USERS uSERS = new USERS();
            uSERS.USERID = uSERID;
            return uSERS;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 USERID
        {
            get
            {
                return _USERID;
            }
            set
            {
                if (_USERID != value)
                {
                    OnUSERIDChanging(value);
                    ReportPropertyChanging("USERID");
                    _USERID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("USERID");
                    OnUSERIDChanged();
                }
            }
        }
        private global::System.Int32 _USERID;
        partial void OnUSERIDChanging(global::System.Int32 value);
        partial void OnUSERIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String EMAIL
        {
            get
            {
                return _EMAIL;
            }
            set
            {
                OnEMAILChanging(value);
                ReportPropertyChanging("EMAIL");
                _EMAIL = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("EMAIL");
                OnEMAILChanged();
            }
        }
        private global::System.String _EMAIL;
        partial void OnEMAILChanging(global::System.String value);
        partial void OnEMAILChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String PASSWORD
        {
            get
            {
                return _PASSWORD;
            }
            set
            {
                OnPASSWORDChanging(value);
                ReportPropertyChanging("PASSWORD");
                _PASSWORD = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("PASSWORD");
                OnPASSWORDChanged();
            }
        }
        private global::System.String _PASSWORD;
        partial void OnPASSWORDChanging(global::System.String value);
        partial void OnPASSWORDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ROLE
        {
            get
            {
                return _ROLE;
            }
            set
            {
                OnROLEChanging(value);
                ReportPropertyChanging("ROLE");
                _ROLE = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("ROLE");
                OnROLEChanged();
            }
        }
        private global::System.String _ROLE;
        partial void OnROLEChanging(global::System.String value);
        partial void OnROLEChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("CardModel", "FK__CARDS__USERID__29572725", "CARDS")]
        public EntityCollection<CARDS> CARDS
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<CARDS>("CardModel.FK__CARDS__USERID__29572725", "CARDS");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<CARDS>("CardModel.FK__CARDS__USERID__29572725", "CARDS", value);
                }
            }
        }

        #endregion

    }

    #endregion

    
}
