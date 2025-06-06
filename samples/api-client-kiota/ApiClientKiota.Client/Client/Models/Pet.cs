// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Clients.Petstore.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Pet : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The category property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Clients.Petstore.Models.Category? Category { get; set; }
#nullable restore
#else
        public global::Clients.Petstore.Models.Category Category { get; set; }
#endif
        /// <summary>The id property</summary>
        public long? Id { get; set; }
        /// <summary>The name property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The photoUrls property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? PhotoUrls { get; set; }
#nullable restore
#else
        public List<string> PhotoUrls { get; set; }
#endif
        /// <summary>pet status in the store</summary>
        public global::Clients.Petstore.Models.Pet_status? Status { get; set; }
        /// <summary>The tags property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Clients.Petstore.Models.Tag>? Tags { get; set; }
#nullable restore
#else
        public List<global::Clients.Petstore.Models.Tag> Tags { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Clients.Petstore.Models.Pet"/> and sets the default values.
        /// </summary>
        public Pet()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Clients.Petstore.Models.Pet"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Clients.Petstore.Models.Pet CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Clients.Petstore.Models.Pet();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "category", n => { Category = n.GetObjectValue<global::Clients.Petstore.Models.Category>(global::Clients.Petstore.Models.Category.CreateFromDiscriminatorValue); } },
                { "id", n => { Id = n.GetLongValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "photoUrls", n => { PhotoUrls = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "status", n => { Status = n.GetEnumValue<global::Clients.Petstore.Models.Pet_status>(); } },
                { "tags", n => { Tags = n.GetCollectionOfObjectValues<global::Clients.Petstore.Models.Tag>(global::Clients.Petstore.Models.Tag.CreateFromDiscriminatorValue)?.AsList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Clients.Petstore.Models.Category>("category", Category);
            writer.WriteLongValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteCollectionOfPrimitiveValues<string>("photoUrls", PhotoUrls);
            writer.WriteEnumValue<global::Clients.Petstore.Models.Pet_status>("status", Status);
            writer.WriteCollectionOfObjectValues<global::Clients.Petstore.Models.Tag>("tags", Tags);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
