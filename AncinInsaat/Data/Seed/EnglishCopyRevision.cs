using System.Text.RegularExpressions;
using AncinInsaat.Data.Entities;
using AncinInsaat.Services;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Data.Seed;

// GENERATED DATA — English copy revision (2026-11-03, client's general
// manager request: natural, corporate British English across the /en site).
//
// Not a seed — an idempotent startup reconcile for every English column
// stored in the database. Each English value is changed only when it is
//   (a) still empty/null (a translation that never existed is filled), or
//   (b) still exactly equal to the previous English text (OldValue below),
// so any later manual edit made on the live database is never overwritten.
// Second pass (same day): the general manager's decisions (Phase names,
// Nlatis, Entrance Wardrobe, …) plus a second review; a value still equal
// to the text the first pass wrote also counts as "previous text" — see
// FirstPassTexts.
// Already-revised rows load and compare without writing anything, so this
// is a no-op on every startup after the first. Runs last in
// DbSeeder.SeedAsync, after every seed/reconcile step that may (re)insert
// rows, so freshly (re)inserted rows receive the new English text too.
public static class EnglishCopyRevision
{
    private record ProjectRevision(string Slug, string Field, string? OldValue, string NewValue);
    private record ConceptRevision(string Slug, string TurkishTitle, string? OldEyebrow, string Eyebrow, string? OldTitle, string Title, string? OldDescription, string Description);
    private record SeoRevision(string Page, string? OldTitle, string Title, string? OldDescription, string Description);
    private record TermRevision(string Turkish, string? OldValue, string NewValue);

    private static readonly ProjectRevision[] Projects =
    {
        new("nysa-gold", "ShortDescriptionEn", "A modern residence offering a privileged life in Aydın Efeler, with elegant façades, a swimming pool and thoughtfully designed social areas.", "A modern residence in Aydın Efeler with elegant façades, a swimming pool and thoughtfully designed social facilities."),
        new("nysa-gold", "DescriptionEn", "Rising in the developing district of Aydın Efeler, Nysa Gold Residence brings modern architecture together with a comfortable, secure way of life. With its swimming pool, basketball court, children's playground and landscaped gardens, the project offers social spaces residents of every age can enjoy at any hour of the day. Every apartment is designed with wide balconies and bright interiors that carry natural light deep inside.", "Set in a growing part of Aydın Efeler, Nysa Gold Residence combines modern architecture with a comfortable, secure way of life. With a swimming pool, basketball court, children's playground and landscaped gardens, the development offers residents of all ages social spaces to enjoy at any time of day. Every apartment has been designed with generous balconies and bright, spacious interiors that draw in natural light."),
        new("nysa-gold", "AmenitiesEn", "Swimming Pool\nBasketball Court\nChildren's Playground\nLandscaped Gardens\nParking\nAydın's First Pet Park", "Swimming Pool\nBasketball Court\nChildren's Playground\nLandscaped Gardens\nCar Park\nAydın's First Pet Park"),
        new("le-jardin", "ShortDescriptionEn", null, "An exclusive residence where modern architecture meets a Mediterranean-inspired garden and pool."),
        new("le-jardin", "DescriptionEn", "Bringing the warm character of the Mediterranean together with modern architecture, Le Jardin welcomes its residents with an inviting atmosphere through its bougainvillea-wrapped entrance and light-toned stone-clad façade. With its private pool, shaded terrace and spacious garden, the project unites the comfort of indoor living with the calm of the outdoors. Located in Aydın Efeler, Le Jardin offers an exclusive villa experience for enjoying both everyday life and hosting your guests.", "Bringing the warmth of the Mediterranean together with modern architecture, Le Jardin welcomes residents with a bougainvillea-framed entrance and a light stone-clad façade. With a private pool, shaded terrace and spacious garden, it combines the comfort of indoor living with the calm of the outdoors. Located in Aydın Efeler, Le Jardin offers an exclusive villa lifestyle, ideal for everyday living and for entertaining guests."),
        new("tralles-gold", "ShortDescriptionEn", "A completed residence with a strong architectural identity, standing out in Aydın Efeler's skyline with its white façades and vertical red accents.", "A completed residence in Aydın Efeler with a strong architectural identity, distinguished on the skyline by its white façades and vertical red accents."),
        new("tralles-gold", "DescriptionEn", "Rising within the fabric of Aydın Efeler, Tralles Gold Residence carries a strong architectural identity that sets it apart from its surroundings at a glance, with white façades and vertical red accents. Lit up in the evening hours, the project takes on a distinctive character in the city's night silhouette, bringing together the simplicity of day with the vibrancy of night. Now complete, the project is offered to its residents as a lasting living space that reflects Ancın İnşaat's commitment to craftsmanship.", "Rising within the urban fabric of Aydın Efeler, Tralles Gold Residence has a strong architectural identity, instantly recognisable by its white façades and vertical red accents. Illuminated in the evening, the façade gives the development a distinctive presence on the city's night skyline, pairing daytime simplicity with night-time vibrancy. Now complete, Tralles Gold Residence offers residents an enduring home that reflects Ancın İnşaat's commitment to craftsmanship."),
        new("nlatis", "ShortDescriptionEn", "A completed residential project in İzmir with a distinctive architectural identity, featuring a curved, timber-toned roofline and a glass-clad façade.", "A completed residential project in İzmir with a distinctive architectural identity, defined by a curved, timber-toned roofline and a glazed façade."),
        new("nlatis", "DescriptionEn", "Completed in İzmir, N-Latis carries a distinctive architectural identity with its curved, timber-toned roofline and glass-clad façade. The layered rhythm created by dark façade panels and vertical timber accents, supported by glass balconies, offers a modern, understated look. The ground-floor social amenity areas form a lively welcome point that connects the building with its surroundings.", "Completed in İzmir, Nlatis has a distinctive architectural identity, defined by its curved, timber-toned roofline and glazed façade. The layered rhythm of dark façade panels and vertical timber accents, complemented by glass balconies, creates a modern, understated look. Social spaces on the ground floor form a lively welcome point that connects the building with its surroundings."),
        new("alinda-gold", "ShortDescriptionEn", "A completed modern residence that brings a new character to Aydın Efeler's skyline with its flowing lines and elegant roofline lighting.", "A completed modern residence that adds a new character to the Aydın Efeler skyline with its flowing lines and elegant roof lighting."),
        new("alinda-gold", "DescriptionEn", "Rising on the Aydın Efeler skyline, Alinda Gold Residence brings a new character to the city's silhouette with its flowing lines and elegant roofline lighting. The pool, walking paths and landscaped areas thoughtfully placed between the blocks give residents the chance to start the day outdoors and end it in calm. With its careful lighting design and open composition, the project's entrance makes Ancın İnşaat's premium approach felt from the very first moment.", "Rising on the Aydın Efeler skyline, Alinda Gold Residence adds a new character to the city with its flowing lines and elegant roof lighting. The pool, walking paths and landscaped areas set between the blocks allow residents to start the day outdoors and end it in tranquillity. With its carefully designed lighting and open composition, the entrance conveys Ancın İnşaat's premium approach from the very first moment."),
        new("magnesia-gold", "ShortDescriptionEn", "A completed residence in Aydın Efeler that puts togetherness first, with extensive landscaped grounds and thoughtfully designed shared living spaces.", "A completed residence in Aydın Efeler that brings people together, with extensive landscaping and thoughtfully designed shared spaces."),
        new("magnesia-gold", "DescriptionEn", "Rising side by side on Aydın's skyline, the Magnesia Gold Residence blocks put togetherness first, with extensive landscaped grounds and thoughtfully designed shared living spaces. Palm-lined walking paths, pools and wide lawns offer a peaceful break at any hour of the day, while the pool surrounds — highlighted by careful lighting in the evening — become an inviting gathering point for residents who want to socialize. Now complete, the project places the harmony of greenery and architecture at the center of everyday life.", "Rising side by side on the Aydın skyline, the blocks of Magnesia Gold Residence are designed to bring people together, with extensive landscaping and thoughtfully designed shared spaces. Palm-lined walking paths, pools and wide lawns offer a peaceful retreat at any time of day, while the beautifully lit pool area becomes an inviting meeting place for residents in the evening. Now complete, the development places the harmony of greenery and architecture at the centre of everyday life."),
        new("la-fiore-karabag", "ShortDescriptionEn", "A completed project offering a calm, private life in Aydın İncirliova, with single-story villas carefully set within a lush pine forest.", "A completed development in Aydın İncirliova offering a calm, private lifestyle, with single-storey villas carefully set within a lush pine forest."),
        new("la-fiore-karabag", "DescriptionEn", "Carefully set within a lush pine forest in Aydın İncirliova, La Fiore Karabağ offers a living environment that prioritizes calm and privacy through its single-story villas. Uninterrupted walking paths and landscaping throughout the development promise a daily life immersed in nature, while the pergola-covered seating areas and fireplaces in each villa's garden bring the indoors and the garden together as one. Notable for its stone-clad façade and timber detailing, the project's entrance secures residents' peace of mind with a guarded entry point.", "Carefully set within a lush pine forest in Aydın İncirliova, La Fiore Karabağ offers a calm, private setting with single-storey villas. Continuous walking paths and landscaping throughout the development bring nature into everyday life, while pergola-covered seating areas and fireplaces in the villa gardens connect indoor and outdoor living. Featuring a stone-clad façade and timber details, the entrance includes a security point that ensures residents' peace of mind."),
        new("la-fiore-karabag-2-etap", "ShortDescriptionEn", "A distinguished development rising in Aydın İncirliova, set among centuries-old olive trees with a villa layout fully integrated into nature.", "An exclusive development in Aydın İncirliova, with villas set among centuries-old olive trees in harmony with nature."),
        new("la-fiore-karabag-2-etap", "DescriptionEn", "Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ 2. Etap comes to life with a site plan that preserves the land's natural slope and character. Woven together with wide walking paths, landscaped squares and shared social areas, the project invites its residents into a life together rather than simply a collection of buildings. With its stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ 2. Etap brings together a peaceful way of life and a strong investment opportunity backed by Karabağ's rising value.", "Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ Phase 2 follows a site plan that preserves the natural slope and character of the land. Linked by wide walking paths, landscaped squares and shared social spaces, the development invites residents to enjoy a sense of community rather than simply a collection of buildings. With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ Phase 2 combines a peaceful way of life with a strong investment opportunity, backed by Karabağ's rising value."),
        new("kuyulu-la-via-villalar-birinci-etap", "ShortDescriptionEn", "An exclusive living environment that brings independent villa architecture — each with its own garden and garage — together with Kuyulu's tranquil character.", "An exclusive development combining detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu."),
        new("kuyulu-la-via-villalar-birinci-etap", "DescriptionEn", "La Via Villalar 1. Etap brings independent villa architecture — each with its own garden and garage — together with Kuyulu's tranquil character. Timber-textured façades, elegant lighting and modern lines redefine Ancın İnşaat's commitment to quality in every villa. Designed with its own roof terrace, private pool and landscaped garden for every villa, the project brings social life into your family's own privacy, combining a peaceful way of life with a strong investment opportunity backed by Kuyulu's rising value.", "La Via Villalar Phase 1 combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa. Designed with a private roof terrace, private pool and landscaped garden for each villa, the development brings social life into the privacy of your own home, combining a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value."),
        new("davutlar-d-latis", "ShortDescriptionEn", null, "An exclusive residence set in the unspoilt nature of Kuşadası, where living meets the healing power of thermal waters."),
        new("davutlar-d-latis", "DescriptionEn", "Set amid Kuşadası's thousands of years of history and natural beauty, Davutlar D Latis brings the region's thermal water sources together with a modern architectural living experience. Complete with its spa, fitness and social amenities, the project offers both everyday comfort and a long-term investment opportunity in one of the Aegean's most sought-after tourism destinations. Brought to life under the Ancın İnşaat guarantee, the project aims to be an exclusive address that brings a peaceful life together with thermal healing.", "Set amid the history and natural beauty of Kuşadası, a region with thousands of years of heritage, Davutlar D Latis brings the area's thermal springs together with a modern architectural living experience. Complete with spa, fitness and social facilities, the development offers both everyday comfort and a long-term investment opportunity in one of the Aegean's most popular tourist destinations. Delivered with the assurance of Ancın İnşaat, the project aims to be an exclusive address where a peaceful life meets the healing power of thermal waters."),
        new("davutlar-d-latis", "ConceptDescriptionEn", null, "Rising at the foot of Dilek Peninsula National Park, close to ancient heritage sites such as Ephesus and the House of the Virgin Mary, Davutlar D Latis brings Davutlar's unique thermal waters together with modern architecture. Complete with spa, fitness and social spaces, this wellness concept combines a peaceful lifestyle with a strong investment opportunity in the tourism capital of the Aegean."),
        new("ferhunde-hanim-apt", "ShortDescriptionEn", "A completed apartment project that stands out in Aydın Efeler for its softly curved balconies and carefully chosen façade texture.", "A completed apartment building in Aydın Efeler, distinguished by softly curved balconies and a carefully chosen façade finish."),
        new("ferhunde-hanim-apt", "DescriptionEn", "Ferhunde Hanım Apt. presents an architectural identity that stands out in Aydın Efeler with its softly curved balconies and carefully chosen façade texture. With large windows and spacious balconies, every floor carries natural light inside, while the private garden area enclosed by tall hedges gives residents a safe, peaceful outdoor life away from the noise of the city. Complete with its perimeter walls and landscaped front garden, the project offers an investment opportunity that makes everyday life easier while holding its value.", "Ferhunde Hanım Apt. presents a distinctive architectural identity in Aydın Efeler, with softly curved balconies and a carefully chosen façade finish. Large windows and spacious balconies bring natural light into every floor, while a private garden enclosed by tall hedges offers residents a safe, peaceful outdoor space away from the noise of the city. Complete with perimeter walls and a landscaped front garden, the building offers an investment opportunity that makes everyday life easier and holds its value."),
        new("kuyulu-avm", "ShortDescriptionEn", "A commercial development rising in Aydın Efeler that turns shopping into a lifestyle experience, with shaded terraces and thoughtfully designed landscaping.", "A commercial development in Aydın Efeler that turns shopping into a lifestyle experience, with shaded terraces and thoughtfully designed landscaping."),
        new("kuyulu-avm", "DescriptionEn", "Rising at the edge of a wide olive grove in Aydın Efeler, La Via Shopping Center turns shopping into a lifestyle experience rather than simply a daily errand. With its shaded terraces, wide walkways and thoughtfully designed landscaping, the project reinterprets Ancın İnşaat's signature quality approach at a commercial scale. Natural stone cladding, timber-lamella textures and lighting that traces a soft line through the night give the façade a lasting, elegant character.", "Rising on the edge of a wide olive grove in Aydın Efeler, La Via AVM turns shopping from a daily necessity into a lifestyle experience. With shaded terraces, wide walkways and thoughtfully designed landscaping, the project reinterprets Ancın İnşaat's signature approach to quality on a commercial scale. Natural stone cladding, timber louvres and lighting that traces a soft line after dark give the façade an enduring, elegant character."),
        new("q-latis", "ShortDescriptionEn", null, "An exclusive development in Kuşadası, distinguished by modern architecture and sea-view living spaces."),
        new("q-latis", "DescriptionEn", "Set amid Kuşadası's unique natural surroundings, Hacıfeyzullah - Q-Latis offers a contemporary architectural language that respects the character of the region, with timber-textured façades and wide glazing. The Aegean Sea view from the upper-floor apartments' wide balconies makes every day feel special, while social amenities such as an open terrace, fitness room and an aero-yoga/pilates studio support an active lifestyle. With its hillside position surrounded by olive groves, the project was designed with a site plan that preserves Kuşadası's natural character.", "Set amid the unique natural surroundings of Kuşadası, Hacıfeyzullah - Q-Latis offers a contemporary architectural language that respects the character of the region, with timber-textured façades and wide glazing. Aegean Sea views from the generous balconies of the upper-floor apartments make every day special, while social facilities such as an open terrace, gym and aerial yoga/Pilates studio support an active lifestyle. Set on a hillside surrounded by olive groves, the development has been designed to preserve the natural character of Kuşadası."),
        new("q-latis", "AmenitiesEn", null, "Open Terrace and Lounge Area\nGym\nAerial Yoga / Pilates Studio"),
    };

    private static readonly ConceptRevision[] ConceptImages =
    {
        new("la-fiore-karabag-2-etap", "Zeytinliklerin İçinde Yükselen Mimari", null, "In the Heart of Karabağ", null, "Architecture Rising Among the Olive Groves", null, "Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ Phase 2 follows a site plan that preserves the natural slope and character of the land. Each block is positioned to blend with the surrounding greenery, reflecting Ancın İnşaat's commitment to quality."),
        new("la-fiore-karabag-2-etap", "Sosyal Yaşamın Kalbinde Bir Rota", null, "Shared Living Spaces", null, "A Route at the Heart of Social Life", null, "Linked by wide walking paths, landscaped squares and shared social spaces, La Fiore Karabağ Phase 2 invites residents to enjoy a sense of community rather than simply a collection of buildings. Paths winding through the olive groves offer a peaceful walk at any time of day."),
        new("la-fiore-karabag-2-etap", "Karabağ'da Ayrıcalıklı Bir Yatırım", null, "Value for Today and Tomorrow", null, "An Exclusive Investment in Karabağ", null, "With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ Phase 2 combines a peaceful way of life with a strong investment opportunity. As Karabağ continues to grow in value, the development promises a lasting investment for the future."),
        new("kuyulu-avm", "Zeytinliklerin Eşiğinde Bir Yaşam Merkezi", null, "Kuyulu's New Centre", null, "A Destination on the Edge of the Olive Groves", null, "Rising on the edge of a wide olive grove in Aydın Efeler, La Via AVM turns shopping from a daily necessity into a lifestyle experience. With shaded terraces, wide walkways and thoughtfully designed landscaping, the project reinterprets Ancın İnşaat's signature approach to quality on a commercial scale."),
        new("kuyulu-avm", "Yürüyüş Kadar Keyifli Bir Cephe", null, "A Distinguished Shopping Route", null, "A Façade as Enjoyable as the Stroll", null, "Ground-floor shopfronts are arranged to create an uninterrupted window-shopping experience along the shaded walkway. Wide, palm-lined pavements invite visitors to browse comfortably from shop to shop."),
        new("kuyulu-avm", "Taş, Ahşap ve Işığın Buluştuğu Cephe", null, "An Elegant Architectural Signature", null, "Where Stone, Timber and Light Meet", null, "Natural stone cladding, timber louvres and lighting that traces a soft line after dark give the façade of La Via AVM an enduring, elegant character. Every corner reflects the premium architectural approach found throughout the project."),
        new("kuyulu-la-via-villalar-birinci-etap", "Kendine Ait Bir Mimari Kimlik", null, "In Tranquil Kuyulu", null, "An Architectural Identity of Its Own", null, "La Via Villalar Phase 1 combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa."),
        new("kuyulu-la-via-villalar-birinci-etap", "Kuyulu'da Ayrıcalıklı Bir Yatırım", null, "Value for Today and Tomorrow", null, "An Exclusive Investment in Kuyulu", null, "With orderly rows of villas, landscaped gardens and lush greenery, La Via Villalar Phase 1 follows a cohesive site plan that combines a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value."),
        new("nysa-gold", "Peyzajla Bütünleşen Yerleşim", "A View From Above", "A View from Above", "A Layout Integrated with the Landscape", "A Layout in Harmony with the Landscape", "With its pool, green spaces and orderly internal circulation routes, Nysa Gold Residence offers a balanced, open site plan even from a bird's-eye view.", "With its pool, green spaces and well-organised circulation routes, Nysa Gold Residence presents a balanced, spacious site plan, even when seen from above."),
        new("nysa-gold", "Yeşilin İçinde Sosyal Alanlar", "Family-Focused Living", "Family-Centred Living", "Social Spaces Within the Greenery", "Social Spaces Amid the Greenery", "Fitted with a children's playground and expansive lawns, the inner garden offers Nysa Gold Residence's residents a safe and enjoyable social living space.", "With a children's playground and expansive lawns, the inner garden gives residents of Nysa Gold Residence a safe, enjoyable space to socialise."),
        new("nysa-gold", "Havuz Başında Huzurlu Anlar", "Sunset Pleasure", "Enjoy the Sunset", "Peaceful Moments by the Pool", "Peaceful Moments by the Pool", "Surrounded by palm trees and sun loungers, the pool area adds a hotel-level relaxation experience to Nysa Gold Residence in the warm tones of sunset.", "Framed by palm trees and sun loungers, the pool area brings hotel-style relaxation to Nysa Gold Residence in the warm tones of sunset."),
        new("nysa-gold", "Işıklarla Aydınlanan Bir Yaşam Alanı", "An Atmosphere for the Night", "Evening Ambience", "A Living Space Lit Up After Dark", "A Living Space Illuminated at Night", "With landscape lighting and a carefully designed pool-area lighting scheme, Nysa Gold Residence offers an inviting, secure atmosphere after dark as well.", "With landscape lighting and a carefully designed lighting scheme around the pool, Nysa Gold Residence offers an inviting, secure atmosphere after dark."),
        new("ferhunde-hanim-apt", "Zarif Bir Cepheyle Yükselen Kimlik", null, "In the Heart of Aydın", null, "An Identity Defined by an Elegant Façade", null, "Ferhunde Hanım Apt. presents a distinctive architectural identity in Aydın Efeler, with softly curved balconies and a carefully chosen façade finish. Large windows and spacious balconies bring natural light into every floor."),
        new("ferhunde-hanim-apt", "Şehrin İçinde Sakin Bir Bahçe", null, "Your Own Green Space", null, "A Quiet Garden in the City", null, "A private garden enclosed by tall hedges offers residents a safe, peaceful outdoor space away from the noise of the city. With palm trees and seating areas, it is a pleasant place to unwind at any time of day."),
        new("alinda-gold", "Gün Batımında Yükselen Kıvrımlı Mimari", null, "A New Silhouette for Aydın Efeler", null, "Curved Architecture Rising at Sunset", null, "Rising on the Aydın Efeler skyline, Alinda Gold Residence adds a new character to the city with its flowing lines and elegant roof lighting. Glowing at dusk, its façades offer a lifestyle that pairs the energy of the day with the calm of the evening."),
        new("magnesia-gold", "Birlikte Yükselen Kuleler, Ortak Bir Vizyon", null, "Aydın's Rising Skyline", null, "Towers Rising Together, a Shared Vision", null, "Rising side by side on the Aydın skyline, the blocks of Magnesia Gold Residence are designed to bring people together, with extensive landscaping and thoughtfully designed shared spaces. Glowing at dusk, the façades express the architectural unity of the development."),
        new("magnesia-gold", "Işıkla Tanımlanan Sosyal Yaşam", null, "A Signature in the Evening Glow", null, "Social Life Defined by Light", null, "In the evening, carefully designed lighting turns the pool area of Magnesia Gold Residence into a stage. This atmosphere creates an inviting meeting place for residents who want to leave the day behind and socialise."),
        new("magnesia-gold", "Yeşilin İçinde Huzurlu Bir Mola", null, "A Garden in Harmony with Nature", null, "A Peaceful Retreat Amid the Greenery", null, "With palm-lined walking paths, pools and wide lawns, the landscaping at Magnesia Gold Residence offers a peaceful retreat at any time of day. The harmony between greenery and architecture places quality of life at the centre of the everyday experience."),
        new("tralles-gold", "Şehrin Kalbinde Yan Yana Yükselen Bloklar", null, "The Three Rising Towers of Efeler", null, "Blocks Rising Side by Side in the Heart of the City", null, "Rising side by side within the urban fabric of Aydın Efeler, the blocks of Tralles Gold Residence have a strong architectural identity, instantly recognisable by their white façades and vertical red accents. This aerial view, taken in daylight, clearly shows the development's direct connection with the city."),
        new("nlatis", "Kıvrımlı Çatı Hattıyla Tanımlanan Bir Karakter", null, "An Evening Silhouette in İzmir", null, "A Character Defined by a Curved Roofline", null, "As light spills out from within in the evening, the curved, timber-toned roofline of Nlatis takes on a distinctive character on the İzmir skyline. Its glazed façade creates an inviting look, drawing the last light of the day into the building."),
        new("nlatis", "Cephede Ritmik Bir Malzeme Dili", null, "A Balanced Harmony of Dark Tones and Timber", null, "A Rhythmic Material Language on the Façade", null, "Combining dark façade panels with vertical timber-toned accents, Nlatis creates a layered architectural rhythm with its glass balconies. The green landscaping in front softly frames the building's clean, modern presence."),
        new("nlatis", "Zemin Kattan Başlayan Sosyal Yaşam", null, "A Façade That Comes Alive at Street Level", null, "Social Life Starting on the Ground Floor", null, "The restaurant and social spaces on the ground floor connect the façade of Nlatis with the street, creating a vibrant welcome point. Stretching beneath the curved roofline, this part of the building keeps it animated at every hour of the day."),
        new("la-fiore-karabag", "Ormanla Bütünleşen Villa Yerleşimi", null, "In the Green Setting of Karabağ", null, "A Villa Development at One with the Forest", null, "La Fiore Karabağ offers a calm, private setting in the Karabağ neighbourhood of İncirliova, Aydın, with single-storey villas carefully set among centuries-old olive trees. Continuous walking paths and landscaping throughout the development bring nature into everyday life."),
        new("la-fiore-karabag", "Bahçeyle Bütünleşen Açık Hava Konforu", null, "Evenings in the Garden", null, "Outdoor Comfort at One with the Garden", null, "Surrounded by stone columns, the large glazed winter garden opens directly onto a garden of lush lawns and flowering plants. Immersed in the landscape, this peaceful outdoor space links day with evening and indoors with outdoors, making relaxation a natural part of daily life at La Fiore Karabağ."),
        new("la-fiore-karabag", "Taş Duvarlarla Çevrili Güvenlikli Giriş", null, "The First Welcome", null, "A Secure Entrance Framed by Stone Walls", null, "At the end of an approach road lined with palms and cypresses, a stone wall bearing the La Fiore Karabağ name welcomes residents and guests. The secure entrance safeguards the privacy and tranquillity of this peaceful development in the Karabağ neighbourhood."),
        new("q-latis", "Doğayla Uyumlu Modern Mimari", null, "Architectural Identity", null, "Modern Architecture in Harmony with Nature", null, "With timber-textured façades, wide glazing and balconies that blend with the greenery, Hacıfeyzullah - Q-Latis offers a contemporary architectural language that respects the character of Kuşadası."),
        new("q-latis", "Balkonunuzdan Ege'nin Eşsiz Manzarası", null, "Aegean Views", null, "Unrivalled Aegean Views from Your Balcony", null, "Aegean Sea views from the generous balconies of the upper-floor apartments make every day special at Hacıfeyzullah - Q-Latis."),
        new("q-latis", "Kuşadası'nın Kalbinde", null, "Location", null, "In the Heart of Kuşadası", null, "Rising on a hillside surrounded by olive groves, Hacıfeyzullah - Q-Latis has been designed to preserve the natural character of Kuşadası."),
        new("nysa-gold", "Aydın'ın İlk Pet Parkı", null, "Living with Pets", null, "Aydın's First Pet Park", null, "Breaking new ground in Aydın, Nysa Gold Residence features a dedicated pet park where you can spend quality time with your pets. This exclusive space offers residents a comfortable, pet-friendly and modern way of life."),
        new("le-jardin", "Taş ve Ahşabın Modern Uyumu", null, "First Impressions", null, "A Modern Harmony of Stone and Timber", null, "With light stone-clad façades, timber louvre details and bougainvillea-framed entrances, the Le Jardin villas bring modern architecture together with Mediterranean greenery along a private stone-paved road."),
        new("le-jardin", "Camla Bütünleşen Modern Cephe", null, "The Villa at Dusk", null, "A Modern Façade Embracing Glass", null, "With timber louvred shutters, a glass-balustraded balcony and a stone-clad ground floor, the villa creates a tranquil scene at dusk, accompanied by the sound of the waterfall feature in its private pool."),
    };

    private static readonly ConceptRevision[] ConceptVideos =
    {
        new("davutlar-d-latis", "Kuşadası'nın Yeni Yaşam Merkezi", null, "Welcome to Kuşadası", null, "Kuşadası's New Centre of Living", null, "Rising at the foot of Dilek Peninsula National Park, close to ancient heritage sites such as Ephesus and the House of the Virgin Mary, Davutlar D Latis offers an exclusive living environment immersed in unspoilt nature."),
        new("davutlar-d-latis", "Termal Wellness Ayrıcalığı", null, "Discover the Healing Waters", null, "The Privilege of Thermal Wellness", null, "Bringing Davutlar's unique thermal waters together with modern architecture, the project offers a holistic wellness concept that brings healthy, tranquil living to every apartment."),
        new("davutlar-d-latis", "Modern Mimari", null, "A Contemporary Signature", null, "Modern Architecture", null, "The contemporary architecture of Blocks A, B and C is complemented by comfort-focused apartment types that suit every lifestyle."),
        new("davutlar-d-latis", "Sosyal Yaşam", null, "Every Day, a Privilege", null, "Social Life", null, "From spa and fitness areas to shared living spaces, the social facilities turn everyday life into a holiday experience."),
        new("davutlar-d-latis", "Prestijli Yatırım", null, "Invest in the Future", null, "A Prestigious Investment", null, "Rising in the tourism capital of the Aegean, Davutlar D Latis combines a peaceful way of life with a strong investment opportunity."),
        new("nysa-gold", "Nysa Gold Residence'ın Mimari Vizyonu", "A New Generation of Living", "A New Generation of Living", "The Architectural Vision of Nysa Gold Residence", "The Architectural Vision of Nysa Gold Residence", "With its elegant façades, wide balconies and thoughtfully designed social areas, Nysa Gold Residence brings modern architecture together with a comfortable living experience.", "With elegant façades, generous balconies and thoughtfully designed social facilities, Nysa Gold Residence brings modern architecture together with comfortable living."),
        new("q-latis", "Hacıfeyzullah - Q-Latis'e Hoş Geldiniz", null, "A New Life in Kuşadası", null, "Welcome to Hacıfeyzullah - Q-Latis", null, "Set amid the unique natural surroundings of Kuşadası and delivered with the assurance of Ancın İnşaat, Hacıfeyzullah - Q-Latis offers an exclusive lifestyle with modern architecture and sea-view living spaces."),
        new("le-jardin", "Begonvillerin Sardığı Giriş", null, "An Inviting Welcome", null, "An Entrance Framed by Bougainvillea", null, "With a natural arch of bougainvillea around the door and a light stone-clad façade, the entrance gives Le Jardin a sophisticated, artistic identity."),
    };

    private static readonly SeoRevision[] Seo =
    {
        new("home", "Ancın İnşaat | Trusted Residential Projects in Aydın", "Ancın İnşaat | Trusted Residential Developments in Aydın", "Ancın İnşaat carries living spaces into the future through residential projects shaped by trust and craftsmanship in Aydın. Explore our ongoing and completed projects.", "Ancın İnşaat creates homes for the future in Aydın, shaped by trust and craftsmanship. Explore our ongoing and completed projects."),
        new("projects", "Our Projects | Ancın İnşaat", "Our Projects | Ancın İnşaat", "Explore all of Ancın İnşaat's completed and ongoing residential projects in Aydın.", "Discover all of Ancın İnşaat's completed and ongoing residential projects in Aydın."),
        new("contact", "Contact | Ancın İnşaat", "Contact Us | Ancın İnşaat", "Get in touch with Ancın İnşaat — our address, phone, email details and contact form.", "Get in touch with Ancın İnşaat: our address, phone number, email and contact form."),
        new("about-us", "About Us | Ancın İnşaat", "About Us | Ancın İnşaat", "Discover Ancın İnşaat's story since 1973, our values and our journey in Aydın.", "Discover the story of Ancın İnşaat since 1973, our values and our journey in Aydın."),
        new("values", "Our Values | Ancın İnşaat", "Our Values | Ancın İnşaat", "Discover the core values that shape Ancın İnşaat's corporate culture and approach to construction.", "Discover the core values that shape Ancın İnşaat's corporate culture and approach to construction."),
        new("career", "Career | Ancın İnşaat", "Careers | Ancın İnşaat", "Explore career opportunities at Ancın İnşaat and apply to our open positions with your CV.", "Explore career opportunities at Ancın İnşaat and apply with your CV."),
        new("hr-policy", "Human Resources Policy | Ancın İnşaat", "Human Resources Policy | Ancın İnşaat", "Discover Ancın İnşaat's approach to employee development, corporate culture and human resources principles.", "Discover Ancın İnşaat's approach to employee development, corporate culture and human resources principles."),
        new("kvkk", "KVKK Privacy Notice | Ancın İnşaat", "Personal Data Protection Notice (KVKK) | Ancın İnşaat", "Ancın İnşaat's notice on the processing of personal data under Turkish Law No. 6698 on the Protection of Personal Data (KVKK).", "Ancın İnşaat's notice on the processing of personal data under the Turkish Personal Data Protection Law No. 6698 (KVKK)."),
        new("nysa-gold", "Nysa Gold Residence | Ancın İnşaat", "Nysa Gold Residence | Ancın İnşaat", "Explore Ancın İnşaat's Nysa Gold Residence project. Location, status and project details.", "Discover Nysa Gold Residence by Ancın İnşaat in Aydın: location, status and project details."),
        new("le-jardin", "Le Jardin | Ancın İnşaat", "Le Jardin | Ancın İnşaat", "Explore Ancın İnşaat's Le Jardin project. Location, status and project details.", "Discover Le Jardin by Ancın İnşaat in Aydın: location, status and project details."),
        new("tralles-gold", "Tralles Gold Residence | Ancın İnşaat", "Tralles Gold Residence | Ancın İnşaat", "Explore Ancın İnşaat's Tralles Gold Residence project. Location, status and project details.", "Discover Tralles Gold Residence by Ancın İnşaat in Aydın: location, status and project details."),
        new("nlatis", "Nlatis | Ancın İnşaat", "Nlatis | Ancın İnşaat", "Explore Ancın İnşaat's Nlatis project. Location, status and project details.", "Discover Nlatis by Ancın İnşaat in Narlıdere, İzmir: location, status and project details."),
        new("alinda-gold", "Alinda Gold Residence | Ancın İnşaat", "Alinda Gold Residence | Ancın İnşaat", "Explore Ancın İnşaat's Alinda Gold Residence project. Location, status and project details.", "Discover Alinda Gold Residence by Ancın İnşaat in Aydın: location, status and project details."),
        new("magnesia-gold", "Magnesia Gold Residence | Ancın İnşaat", "Magnesia Gold Residence | Ancın İnşaat", "Explore Ancın İnşaat's Magnesia Gold Residence project. Location, status and project details.", "Discover Magnesia Gold Residence by Ancın İnşaat in Aydın: location, status and project details."),
        new("la-fiore-karabag", "La Fiore Karabağ | Ancın İnşaat", "La Fiore Karabağ | Ancın İnşaat", "Explore Ancın İnşaat's La Fiore Karabağ project. Location, status and project details.", "Discover La Fiore Karabağ by Ancın İnşaat in Aydın: location, status and project details."),
        new("la-fiore-karabag-2-etap", "La Fiore Karabağ 2. Etap | Ancın İnşaat", "La Fiore Karabağ Phase 2 | Ancın İnşaat", "Explore Ancın İnşaat's La Fiore Karabağ 2. Etap project. Location, status and project details.", "Discover La Fiore Karabağ Phase 2 by Ancın İnşaat in Aydın: location, status and project details."),
        new("davutlar-d-latis", "Davutlar D Latis | Ancın İnşaat", "Davutlar D Latis | Ancın İnşaat", "Explore Ancın İnşaat's Davutlar D Latis project. Location, status and project details.", "Discover Davutlar D Latis by Ancın İnşaat in Aydın: location, status and project details."),
        new("ferhunde-hanim-apt", "Ferhunde Hanım Apt. | Ancın İnşaat", "Ferhunde Hanım Apt. | Ancın İnşaat", "Explore Ancın İnşaat's Ferhunde Hanım Apt. project. Location, status and project details.", "Discover Ferhunde Hanım Apt. by Ancın İnşaat in Aydın: location, status and project details."),
        new("kuyulu-la-via-villalar-birinci-etap", "La Via Villalar 1. Etap | Ancın İnşaat", "La Via Villalar Phase 1 | Ancın İnşaat", "Explore Ancın İnşaat's La Via Villalar 1. Etap project. Location, status and project details.", "Discover La Via Villalar Phase 1 by Ancın İnşaat in Aydın: location, status and project details."),
        new("q-latis", "Hacıfeyzullah - Q-Latis | Ancın İnşaat", "Hacıfeyzullah - Q-Latis | Ancın İnşaat", "Explore Ancın İnşaat's Hacıfeyzullah - Q-Latis project. Location, status and project details.", "Discover Hacıfeyzullah - Q-Latis by Ancın İnşaat in Aydın: location, status and project details."),
        new("sosyal-sorumluluk-projelerimiz", "Social Responsibility | Ancın İnşaat", "Social Responsibility | Ancın İnşaat", "Discover the approach that shapes Ancın İnşaat's sense of responsibility toward its employees, the environment and the local community.", "Discover how Ancın İnşaat fulfils its responsibilities towards its employees, the environment and the local community."),
    };

    // Keyed by the Turkish value, shared by every project.
    private static readonly TermRevision[] ApartmentTypes =
    {
        new("Zemin Kat", null, "Ground Floor"),
        new("1. Kat", null, "First Floor"),
        new("Bodrum Kat", null, "Basement"),
        new("Planlar yakında eklenecektir.", null, "Floor plans will be available soon."),
        new("A Tipi Blok – Zemin Kat", null, "Type A Block – Ground Floor"),
        new("A Tipi Blok – 1. Kat", null, "Type A Block – First Floor"),
        new("A Tipi Blok – 1+1", null, "Type A Block – 1+1 (1-Bedroom)"),
        new("B Tipi Blok – Zemin Kat", null, "Type B Block – Ground Floor"),
        new("B Tipi Blok – 1. Kat", null, "Type B Block – First Floor"),
        new("B Tipi Blok (3+1) – Zemin Kat", null, "Type B Block (3+1) – Ground Floor"),
        new("B Tipi Blok (3+1) – 1. Kat", null, "Type B Block (3+1) – First Floor"),
        new("C Tipi Blok – Zemin Kat", null, "Type C Block – Ground Floor"),
        new("C Tipi Blok – 1. Kat", null, "Type C Block – First Floor"),
        new("C Tipi Blok (3+1) – Zemin Kat", null, "Type C Block (3+1) – Ground Floor"),
        new("C Tipi Blok (3+1) – 1. Kat", null, "Type C Block (3+1) – First Floor"),
        new("D Tipi Blok – Zemin Kat", null, "Type D Block – Ground Floor"),
        new("D Tipi Blok – 1. Kat", null, "Type D Block – First Floor"),
        new("E Tipi Blok – Zemin Kat", null, "Type E Block – Ground Floor"),
        new("E Tipi Blok – 1. Kat", null, "Type E Block – First Floor"),
        new("F Tipi Blok", null, "Type F Block"),
        new("G Tipi Blok (1+1)", null, "Type G Block (1+1)"),
        new("H Tipi Blok – Zemin Kat", null, "Type H Block – Ground Floor"),
        new("H Tipi Blok – 1. Kat", null, "Type H Block – First Floor"),
        new("Standart Kat Planları – Zemin Kat", null, "Standard Floor Plans – Ground Floor"),
        new("Standart Kat Planları – 1. Kat", null, "Standard Floor Plans – First Floor"),
        new("A Tipi Daire", null, "Type A Apartment"),
        new("B Tipi Daire", null, "Type B Apartment"),
        new("C Tipi Daire (1, 2 ve 4. Kat)", null, "Type C Apartment (1st, 2nd and 4th Floors)"),
        new("C Tipi Daire (3. Kat)", null, "Type C Apartment (3rd Floor)"),
        new("D Tipi Daire", null, "Type D Apartment"),
        new("2+1", null, "2+1 (2-Bedroom)"),
        new("3+1", null, "3+1 (3-Bedroom)"),
        new("4+1 Daire | Zemin Kat (Kış Bahçeli)", "4+1 Apartment | Ground Floor (Winter Garden)", "4+1 Apartment | Ground Floor (Winter Garden)"),
        new("3+1 Daire | Zemin Kat (Kış Bahçeli)", "3+1 Apartment | Ground Floor (Winter Garden)", "3+1 Apartment | Ground Floor (Winter Garden)"),
        new("2+1 Daire | Zemin Kat (Kış Bahçeli)", "2+1 Apartment | Ground Floor (Winter Garden)", "2+1 Apartment | Ground Floor (Winter Garden)"),
        new("1+1 Daire | Zemin Kat (Kış Bahçeli)", "1+1 Apartment | Ground Floor (Winter Garden)", "1+1 Apartment | Ground Floor (Winter Garden)"),
        new("4+1 Daire | Kat 1 (Kat 2,3)", "4+1 Apartment | Floor 1 (Floors 2,3)", "4+1 Apartment | Floors 1–3"),
        new("3+1 Daire | Kat 1 (Kat 2,3)", "3+1 Apartment | Floor 1 (Floors 2,3)", "3+1 Apartment | Floors 1–3"),
        new("2+1 Daire | Kat 1 (Kat 2,3,4,5,6,7)", "2+1 Apartment | Floor 1 (Floors 2,3,4,5,6,7)", "2+1 Apartment | Floors 1–7"),
        new("1+1 Daire | Kat 1 (Kat 2,3,4,5,6,7)", "1+1 Apartment | Floor 1 (Floors 2,3,4,5,6,7)", "1+1 Apartment | Floors 1–7"),
        new("3+1 B Tipi Daire | Kat 4 (Kat 5)", "3+1 Type B Apartment | Floor 4 (Floor 5)", "3+1 Type B Apartment | Floors 4–5"),
        new("2+1 B Tipi Daire | Kat 4 (Kat 5)", "2+1 Type B Apartment | Floor 4 (Floor 5)", "2+1 Type B Apartment | Floors 4–5"),
        new("2+1 C Tipi Daire | Kat 6", "2+1 Type C Apartment | Floor 6", "2+1 Type C Apartment | Floor 6"),
        new("2+1 D Tipi Daire | Kat 7", "2+1 Type D Apartment | Floor 7", "2+1 Type D Apartment | Floor 7"),
        new("2+1 E Tipi Daire | Kat 6", "2+1 Type E Apartment | Floor 6", "2+1 Type E Apartment | Floor 6"),
        new("1+1 B Tipi Daire | Kat 7", "1+1 Type B Apartment | Floor 7", "1+1 Type B Apartment | Floor 7"),
        new("A Blok – 2. Bodrum Katı", null, "Block A – Basement Level 2"),
        new("A Blok – 1. Bodrum Katı", null, "Block A – Basement Level 1"),
        new("A Blok – Zemin Kat", null, "Block A – Ground Floor"),
        new("A Blok – 1. Kat", null, "Block A – First Floor"),
        new("A Blok – 2. Kat", null, "Block A – Second Floor"),
        new("A Blok – 3. Kat", null, "Block A – Third Floor"),
        new("A Blok – Çatı Katı", null, "Block A – Roof Level"),
        new("B Blok – 2. Bodrum Katı", null, "Block B – Basement Level 2"),
        new("B Blok – 1. Bodrum Katı", null, "Block B – Basement Level 1"),
        new("B Blok – Zemin Kat", null, "Block B – Ground Floor"),
        new("B Blok – 1. Kat", null, "Block B – First Floor"),
        new("B Blok – 2. Kat", null, "Block B – Second Floor"),
        new("B Blok – 3. Kat", null, "Block B – Third Floor"),
        new("B Blok – Çatı Katı", null, "Block B – Roof Level"),
        new("C Blok – 2. Bodrum Katı", null, "Block C – Basement Level 2"),
        new("C Blok – 1. Bodrum Katı", null, "Block C – Basement Level 1"),
        new("C Blok – Zemin Kat", null, "Block C – Ground Floor"),
        new("C Blok – 1. Kat", null, "Block C – First Floor"),
        new("C Blok – 2. Kat", null, "Block C – Second Floor"),
        new("C Blok – 3. Kat", null, "Block C – Third Floor"),
        new("C Blok – Çatı Katı", null, "Block C – Roof Level"),
    };

    private static readonly TermRevision[] RoomNames =
    {
        new("Giriş", null, "Entrance"),
        new("Antre", null, "Entrance Hall"),
        new("Banyo", "Bathroom", "Bathroom"),
        new("Banyo 1", null, "Bathroom 1"),
        new("Banyo 2", null, "Bathroom 2"),
        new("Banyo 3", null, "Bathroom 3"),
        new("Genel Banyo", "Bathroom", "Family Bathroom"),
        new("Yatak Odası", "Bedroom", "Bedroom"),
        new("Yatak Odası 1", null, "Bedroom 1"),
        new("Yatak Odası 2", null, "Bedroom 2"),
        new("Yatak Odası 3", null, "Bedroom 3"),
        new("Yatak/Çocuk Odası", "Bedroom/Children's Room", "Bedroom / Children's Room"),
        new("Asansör Boşluğu", null, "Lift Shaft"),
        new("Merdiven", null, "Staircase"),
        new("Salon", "Living Room", "Living Room"),
        new("Salon + Mutfak", "Living Room + Kitchen", "Living Room + Kitchen"),
        new("Mutfak", "Kitchen", "Kitchen"),
        new("Teras", null, "Terrace"),
        new("Teras I", "Terrace I", "Terrace I"),
        new("Teras II", "Terrace II", "Terrace II"),
        new("Kış Bahçesi", "Winter Garden", "Winter Garden"),
        new("Bahçe", "Garden", "Garden"),
        new("Havuz", null, "Swimming Pool"),
        new("Ebeveyn Yatak Odası", null, "Master Bedroom"),
        new("E.Yatak Odası", "Master Bedroom", "Master Bedroom"),
        new("Ebeveyn Banyo", null, "En-suite Bathroom"),
        new("E.Banyo", "Master Bathroom", "En-suite Bathroom"),
        new("Hol", "Hall", "Hallway"),
        new("Galeri Boşluğu", null, "Gallery Void"),
        new("Balkon", "Balcony", "Balcony"),
        new("Balkon 1", null, "Balcony 1"),
        new("Balkon 2", null, "Balcony 2"),
        new("Yaşama Alanı", null, "Living Area"),
        new("Yaşam Alanı", null, "Living Area"),
        new("Giriş Holü", "Entrance Hall", "Entrance Hall"),
        new("Giriş Holü 1", null, "Entrance Hall 1"),
        new("Giriş Holü 2", null, "Entrance Hall 2"),
        new("WC", null, "WC"),
        new("Çamaşır Odası", "Laundry Room", "Utility Room"),
        new("Giyinme Odası", "Dressing Room", "Walk-in Wardrobe"),
        new("Vestiyer Odası", "Cloakroom", "Entrance Wardrobe"),
        new("Vestiyer O.", null, "Entrance Wardrobe"),
        new("Kiler", null, "Pantry"),
        new("Oturma Odası", null, "Sitting Room"),
    };

    private static readonly TermRevision[] NearbyPlaceNames =
    {
        new("Atatürk Eğitim Ve Araştırma Hastanesi Narlıdere Semt Polikliniği", null, "Atatürk Training and Research Hospital – Narlıdere Outpatient Clinic"),
        new("Atatürk Kent Meydanı", null, "Atatürk City Square"),
        new("Aydın Atatürk Kent Meydanı", null, "Aydın Atatürk City Square"),
        new("Aydın Adnan Menderes Üniversitesi", null, "Aydın Adnan Menderes University"),
        new("Aydın Atatürk Devlet Hastanesi", null, "Aydın Atatürk State Hospital"),
        new("Aydın Bil Koleji", null, "Aydın Bil Koleji"),
        new("Aydın Otogar", null, "Aydın Bus Station"),
        new("Aydın Polis Meslek Yüksekokulu", null, "Aydın Police Vocational School"),
        new("Aydın Tren Garı", null, "Aydın Railway Station"),
        new("Aydın Çıldır Havalimanı", null, "Aydın Çıldır Airport"),
        new("Aydın Şehir Hastanesi", null, "Aydın City Hospital"),
        new("Dokuz Eylül Üniversitesi Hastanesi", null, "Dokuz Eylül University Hospital"),
        new("Forum Aydın", null, "Forum Aydın Shopping Centre"),
        new("Forum Aydın AVM", null, "Forum Aydın Shopping Centre"),
        new("Kuyulu Aile Sağlığı Merkezi", null, "Kuyulu Family Health Centre"),
        new("Tralleis Antik Kenti", null, "Ancient City of Tralleis"),
        new("Örsdemir Balkan İlkokulu", null, "Örsdemir Balkan Primary School"),
        new("İnciraltı Kent Ormanı", null, "İnciraltı Urban Forest"),
        new("İncirliova Tren İstasyonu", null, "İncirliova Railway Station"),
        new("İncirliova İlçe Merkezi", null, "İncirliova Town Centre"),
        new("İstinye Park İzmir", null, "İstinye Park İzmir"),
        new("İzmir Ekonomi Üniversitesi", null, "İzmir University of Economics"),
        new("Şehir Hastanesi Tren İstasyonu", null, "City Hospital Railway Station"),
        new("Şehitlik Metro İstasyonu", null, "Şehitlik Metro Station"),
    };

    private static readonly TermRevision[] SitePlanAltTexts =
    {
        new("La Fiore Karabağ 2. Etap vaziyet planı 1", null, "La Fiore Karabağ Phase 2 site plan 1"),
        new("La Fiore Karabağ 2. Etap vaziyet planı 2", null, "La Fiore Karabağ Phase 2 site plan 2"),
        new("La Fiore Karabağ 2. Etap vaziyet planı 3", null, "La Fiore Karabağ Phase 2 site plan 3"),
        new("La Via AVM vaziyet planı", null, "La Via AVM site plan"),
        new("Le Jardin vaziyet planı", null, "Le Jardin site plan"),
        new("Nysa Gold Residence vaziyet planı", "Nysa Gold Residence site plan", "Nysa Gold Residence site plan"),
        new("Nysa Gold Residence arsa ve yerleşim görünümü", "Nysa Gold Residence plot and layout view", "Nysa Gold Residence plot and layout view"),
        new("Nysa Gold Residence çevresel konum görünümü", "Nysa Gold Residence surrounding location view", "Nysa Gold Residence location context view"),
        new("Ferhunde Hanım Apt. vaziyet planı", null, "Ferhunde Hanım Apt. site plan"),
        new("La Fiore Karabağ 2. Etap vaziyet planı 4", null, "La Fiore Karabağ Phase 2 site plan 4"),
        new("La Fiore Karabağ vaziyet planı", null, "La Fiore Karabağ site plan"),
        new("La Via Villalar 1. Etap vaziyet planı 1", null, "La Via Villalar Phase 1 site plan 1"),
        new("La Via Villalar 1. Etap vaziyet planı 2", null, "La Via Villalar Phase 1 site plan 2"),
    };

    // Second pass (2026-11-03, general manager's decisions + second review):
    // the English text the first pass wrote, paired with the final text that
    // replaces it. Revise() accepts a current value equal to the first-pass
    // text as well as the original OldValue, but only for the record whose
    // final text is the paired one, so the committed database and any
    // database that already received the first pass both end on the final
    // wording, while a manual edit still never matches and is kept.
    private static readonly HashSet<(string FirstPass, string Final)> FirstPassTexts = new()
    {
        ("Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ 2. Etap follows a site plan that preserves the natural slope and character of the land. Linked by wide walking paths, landscaped squares and shared social spaces, the development invites residents to enjoy a sense of community rather than simply a collection of buildings. With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ 2. Etap combines a peaceful way of life with a strong investment opportunity, backed by Karabağ's rising value.", "Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ Phase 2 follows a site plan that preserves the natural slope and character of the land. Linked by wide walking paths, landscaped squares and shared social spaces, the development invites residents to enjoy a sense of community rather than simply a collection of buildings. With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ Phase 2 combines a peaceful way of life with a strong investment opportunity, backed by Karabağ's rising value."),
        ("La Via Villalar 1. Etap combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa. Designed with a private roof terrace, private pool and landscaped garden for each villa, the development brings social life into the privacy of your own home, combining a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value.", "La Via Villalar Phase 1 combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa. Designed with a private roof terrace, private pool and landscaped garden for each villa, the development brings social life into the privacy of your own home, combining a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value."),
        ("Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ 2. Etap follows a site plan that preserves the natural slope and character of the land. Each block is positioned to blend with the surrounding greenery, reflecting Ancın İnşaat's commitment to quality.", "Set among centuries-old olive trees in Aydın Karabağ, La Fiore Karabağ Phase 2 follows a site plan that preserves the natural slope and character of the land. Each block is positioned to blend with the surrounding greenery, reflecting Ancın İnşaat's commitment to quality."),
        ("Linked by wide walking paths, landscaped squares and shared social spaces, La Fiore Karabağ 2. Etap invites residents to enjoy a sense of community rather than simply a collection of buildings. Paths winding through the olive groves offer a peaceful walk at any time of day.", "Linked by wide walking paths, landscaped squares and shared social spaces, La Fiore Karabağ Phase 2 invites residents to enjoy a sense of community rather than simply a collection of buildings. Paths winding through the olive groves offer a peaceful walk at any time of day."),
        ("With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ 2. Etap combines a peaceful way of life with a strong investment opportunity. As Karabağ continues to grow in value, the development promises a lasting investment for the future.", "With stone walls, elegant lighting and villas surrounded by olive trees, La Fiore Karabağ Phase 2 combines a peaceful way of life with a strong investment opportunity. As Karabağ continues to grow in value, the development promises a lasting investment for the future."),
        ("La Via Villalar 1. Etap combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa.", "La Via Villalar Phase 1 combines detached villa architecture, each villa with its own garden and garage, with the tranquil character of Kuyulu. Timber-textured façades, elegant lighting and clean, modern lines express Ancın İnşaat's commitment to quality in every villa."),
        ("With orderly rows of villas, landscaped gardens and lush greenery, La Via Villalar 1. Etap follows a cohesive site plan that combines a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value.", "With orderly rows of villas, landscaped gardens and lush greenery, La Via Villalar Phase 1 follows a cohesive site plan that combines a peaceful way of life with a strong investment opportunity, backed by Kuyulu's rising value."),
        ("The restaurant and social spaces on the ground floor connect the façade of Nlatis with the street, creating a lively welcome point. Stretching beneath the curved roofline, this part of the building gives it a lively presence at every hour of the day.", "The restaurant and social spaces on the ground floor connect the façade of Nlatis with the street, creating a vibrant welcome point. Stretching beneath the curved roofline, this part of the building keeps it animated at every hour of the day."),
        ("A Modern Façade at One with Glass", "A Modern Façade Embracing Glass"),
        ("Explore career opportunities at Ancın İnşaat and apply for our open positions with your CV.", "Explore career opportunities at Ancın İnşaat and apply with your CV."),
        ("Discover Nlatis by Ancın İnşaat: location, status and project details.", "Discover Nlatis by Ancın İnşaat in Narlıdere, İzmir: location, status and project details."),
        ("La Fiore Karabağ 2. Etap | Ancın İnşaat", "La Fiore Karabağ Phase 2 | Ancın İnşaat"),
        ("Discover La Fiore Karabağ 2. Etap by Ancın İnşaat in Aydın: location, status and project details.", "Discover La Fiore Karabağ Phase 2 by Ancın İnşaat in Aydın: location, status and project details."),
        ("La Via Villalar 1. Etap | Ancın İnşaat", "La Via Villalar Phase 1 | Ancın İnşaat"),
        ("Discover La Via Villalar 1. Etap by Ancın İnşaat in Aydın: location, status and project details.", "Discover La Via Villalar Phase 1 by Ancın İnşaat in Aydın: location, status and project details."),
        ("Block A – Roof Floor", "Block A – Roof Level"),
        ("Block B – Roof Floor", "Block B – Roof Level"),
        ("Block C – Roof Floor", "Block C – Roof Level"),
        ("Bathroom", "Family Bathroom"),
        ("Walk-in Wardrobe", "Entrance Wardrobe"),
        ("Aydın Bil College", "Aydın Bil Koleji"),
        ("Forum Aydın", "Forum Aydın Shopping Centre"),
        ("La Fiore Karabağ 2. Etap site plan 1", "La Fiore Karabağ Phase 2 site plan 1"),
        ("La Fiore Karabağ 2. Etap site plan 2", "La Fiore Karabağ Phase 2 site plan 2"),
        ("La Fiore Karabağ 2. Etap site plan 3", "La Fiore Karabağ Phase 2 site plan 3"),
        ("La Fiore Karabağ 2. Etap site plan 4", "La Fiore Karabağ Phase 2 site plan 4"),
        ("La Via Villalar 1. Etap site plan 1", "La Via Villalar Phase 1 site plan 1"),
        ("La Via Villalar 1. Etap site plan 2", "La Via Villalar Phase 1 site plan 2"),
    };

    // AltTextRules patterns whose output the second pass changed, mapped to
    // the first-pass output, so an alt text the first pass already filled is
    // recognised and upgraded (see ApplyAsync).
    private static readonly Dictionary<string, string> FirstPassAltTextReplacements = new()
    {
        ["^1\\. Etap basketbol sahası görünümü$"] = "1. Etap basketball court",
        ["^1\\. Etap dekoratif havuz ve çevresindeki peyzaj düzenlemesi$"] = "1. Etap ornamental pool and surrounding landscaping",
        ["^1\\. Etap güneş enerjili karport ve elektrikli araç şarj istasyonu$"] = "1. Etap solar carport and electric vehicle charging station",
        ["^1\\. Etap seyir terası ve peyzajlı çevre düzenlemesi$"] = "1. Etap viewing terrace and landscaped surroundings",
        ["^1\\. Etap yürüyüş yolu ve peyzajlı çevre düzenlemesi$"] = "1. Etap walking path and landscaped surroundings",
        ["^Standart İç Mekan – Vestiyer Odası (\\d+)$"] = "standard interior – walk-in wardrobe $1",
        ["^Standart İç Mekan – Vestiyer Odası$"] = "standard interior – walk-in wardrobe",
        ["^Opsiyonel İç Mekan – Vestiyer Odası (\\d+)$"] = "optional interior – walk-in wardrobe $1",
        ["^Opsiyonel İç Mekan – Vestiyer Odası$"] = "optional interior – walk-in wardrobe",
    };

    private const string OldWorkingHoursEn = "Monday - Friday, 09:00 - 18:00";
    private const string WorkingHoursEn = "Monday to Friday, 09:00–18:00";

    // ProjectImage.AltText → AltTextEn, applied to the text after the
    // project-name prefix ("<Project.Name> <rest>"). Only ever fills an
    // empty AltTextEn; an alt text no rule recognises is left untouched.
    private static readonly (Regex Pattern, string Replacement)[] AltTextRules =
    {
        (new Regex("^Kuşadası tepelerinden dış cephe görünümü$", RegexOptions.CultureInvariant), "exterior view from the Kuşadası hills"),
        (new Regex("^aeroyoga stüdyosu$", RegexOptions.CultureInvariant), "aerial yoga studio"),
        (new Regex("^ahşap cephe detayı$", RegexOptions.CultureInvariant), "timber façade detail"),
        (new Regex("^akşam ışığında dış cephe ve deniz manzarası$", RegexOptions.CultureInvariant), "exterior and sea view in the evening light"),
        (new Regex("^açık teras ve fitness alanı$", RegexOptions.CultureInvariant), "open terrace and fitness area"),
        (new Regex("^balkon ve cephe detayı$", RegexOptions.CultureInvariant), "balcony and façade detail"),
        (new Regex("^fitness ekipmanları$", RegexOptions.CultureInvariant), "fitness equipment"),
        (new Regex("^gece dış cephe görünümü$", RegexOptions.CultureInvariant), "exterior view at night"),
        (new Regex("^gün batımında balkon detayı$", RegexOptions.CultureInvariant), "balcony detail at sunset"),
        (new Regex("^gün batımında balkon ve deniz manzarası$", RegexOptions.CultureInvariant), "balcony and sea view at sunset"),
        (new Regex("^kuşbakışı yerleşim görünümü$", RegexOptions.CultureInvariant), "aerial view of the site layout"),
        (new Regex("^salon iç mekan görünümü$", RegexOptions.CultureInvariant), "living room interior view"),
        (new Regex("^sokak seviyesinden dış cephe görünümü$", RegexOptions.CultureInvariant), "exterior view from street level"),
        (new Regex("^çatı terası ve yüzme havuzu görünümü$", RegexOptions.CultureInvariant), "roof terrace and swimming pool view"),
        (new Regex("^sosyal alan görünümü \\(yakında eklenecek\\)$", RegexOptions.CultureInvariant), "social facilities view (coming soon)"),
        (new Regex("^1\\. Etap basketbol sahası görünümü$", RegexOptions.CultureInvariant), "Phase 1 basketball court"),
        (new Regex("^1\\. Etap dekoratif havuz ve çevresindeki peyzaj düzenlemesi$", RegexOptions.CultureInvariant), "Phase 1 ornamental pool and surrounding landscaping"),
        (new Regex("^1\\. Etap güneş enerjili karport ve elektrikli araç şarj istasyonu$", RegexOptions.CultureInvariant), "Phase 1 solar carport and electric vehicle charging station"),
        (new Regex("^1\\. Etap seyir terası ve peyzajlı çevre düzenlemesi$", RegexOptions.CultureInvariant), "Phase 1 viewing terrace and landscaped surroundings"),
        (new Regex("^1\\. Etap yürüyüş yolu ve peyzajlı çevre düzenlemesi$", RegexOptions.CultureInvariant), "Phase 1 walking path and landscaped surroundings"),
        (new Regex("^([A-Z]) Tip iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "Type $1 interior view $2"),
        (new Regex("^([A-Z]) Tipi Blok dış cephe görünümü (\\d+)$", RegexOptions.CultureInvariant), "Type $1 Block exterior view $2"),
        (new Regex("^([A-Z]) Tipi Blok iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "Type $1 Block interior view $2"),
        (new Regex("^([A-Z]) Tipi Blok Sağ Tip iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "Type $1 Block right-hand unit interior view $2"),
        (new Regex("^([A-Z]) Tipi Blok Sol Tip iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "Type $1 Block left-hand unit interior view $2"),
        (new Regex("^(\\d\\+\\d) ([A-Z]) Tipi iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "$1 Type $2 interior view $3"),
        (new Regex("^(\\d\\+\\d) Tipi iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "$1 interior view $2"),
        (new Regex("^(\\d\\+\\d) Tipi tanıtım videosu (\\d+)$", RegexOptions.CultureInvariant), "$1 promotional video $2"),
        (new Regex("^Standart İç Mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "standard interior view $1"),
        (new Regex("^Standart İç Mekan – Ebeveyn Banyosu (\\d+)$", RegexOptions.CultureInvariant), "standard interior – en-suite bathroom $1"),
        (new Regex("^Standart İç Mekan – Ebeveyn Banyosu$", RegexOptions.CultureInvariant), "standard interior – en-suite bathroom"),
        (new Regex("^Standart İç Mekan – Ebeveyn Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "standard interior – master bedroom $1"),
        (new Regex("^Standart İç Mekan – Ebeveyn Yatak Odası$", RegexOptions.CultureInvariant), "standard interior – master bedroom"),
        (new Regex("^Standart İç Mekan – Ebeveyn Odası (\\d+)$", RegexOptions.CultureInvariant), "standard interior – master bedroom $1"),
        (new Regex("^Standart İç Mekan – Ebeveyn Odası$", RegexOptions.CultureInvariant), "standard interior – master bedroom"),
        (new Regex("^Standart İç Mekan – Giyinme Odası (\\d+)$", RegexOptions.CultureInvariant), "standard interior – walk-in wardrobe $1"),
        (new Regex("^Standart İç Mekan – Giyinme Odası$", RegexOptions.CultureInvariant), "standard interior – walk-in wardrobe"),
        (new Regex("^Standart İç Mekan – Vestiyer Odası (\\d+)$", RegexOptions.CultureInvariant), "standard interior – entrance wardrobe $1"),
        (new Regex("^Standart İç Mekan – Vestiyer Odası$", RegexOptions.CultureInvariant), "standard interior – entrance wardrobe"),
        (new Regex("^Standart İç Mekan – Yatak Odası Banyo (\\d+)$", RegexOptions.CultureInvariant), "standard interior – bedroom bathroom $1"),
        (new Regex("^Standart İç Mekan – Yatak Odası Banyo$", RegexOptions.CultureInvariant), "standard interior – bedroom bathroom"),
        (new Regex("^Standart İç Mekan – Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "standard interior – bedroom $1"),
        (new Regex("^Standart İç Mekan – Yatak Odası$", RegexOptions.CultureInvariant), "standard interior – bedroom"),
        (new Regex("^Standart İç Mekan – Giriş (\\d+)$", RegexOptions.CultureInvariant), "standard interior – entrance $1"),
        (new Regex("^Standart İç Mekan – Giriş$", RegexOptions.CultureInvariant), "standard interior – entrance"),
        (new Regex("^Standart İç Mekan – Hol (\\d+)$", RegexOptions.CultureInvariant), "standard interior – hallway $1"),
        (new Regex("^Standart İç Mekan – Hol$", RegexOptions.CultureInvariant), "standard interior – hallway"),
        (new Regex("^Standart İç Mekan – Kiler (\\d+)$", RegexOptions.CultureInvariant), "standard interior – pantry $1"),
        (new Regex("^Standart İç Mekan – Kiler$", RegexOptions.CultureInvariant), "standard interior – pantry"),
        (new Regex("^Standart İç Mekan – Mutfak (\\d+)$", RegexOptions.CultureInvariant), "standard interior – kitchen $1"),
        (new Regex("^Standart İç Mekan – Mutfak$", RegexOptions.CultureInvariant), "standard interior – kitchen"),
        (new Regex("^Standart İç Mekan – Oda (\\d+)$", RegexOptions.CultureInvariant), "standard interior – room $1"),
        (new Regex("^Standart İç Mekan – Oda$", RegexOptions.CultureInvariant), "standard interior – room"),
        (new Regex("^Standart İç Mekan – Salon (\\d+)$", RegexOptions.CultureInvariant), "standard interior – living room $1"),
        (new Regex("^Standart İç Mekan – Salon$", RegexOptions.CultureInvariant), "standard interior – living room"),
        (new Regex("^Standart İç Mekan – WC (\\d+)$", RegexOptions.CultureInvariant), "standard interior – WC $1"),
        (new Regex("^Standart İç Mekan – WC$", RegexOptions.CultureInvariant), "standard interior – WC"),
        (new Regex("^Opsiyonel İç Mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "optional interior view $1"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Banyosu (\\d+)$", RegexOptions.CultureInvariant), "optional interior – en-suite bathroom $1"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Banyosu$", RegexOptions.CultureInvariant), "optional interior – en-suite bathroom"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "optional interior – master bedroom $1"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Yatak Odası$", RegexOptions.CultureInvariant), "optional interior – master bedroom"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Odası (\\d+)$", RegexOptions.CultureInvariant), "optional interior – master bedroom $1"),
        (new Regex("^Opsiyonel İç Mekan – Ebeveyn Odası$", RegexOptions.CultureInvariant), "optional interior – master bedroom"),
        (new Regex("^Opsiyonel İç Mekan – Giyinme Odası (\\d+)$", RegexOptions.CultureInvariant), "optional interior – walk-in wardrobe $1"),
        (new Regex("^Opsiyonel İç Mekan – Giyinme Odası$", RegexOptions.CultureInvariant), "optional interior – walk-in wardrobe"),
        (new Regex("^Opsiyonel İç Mekan – Vestiyer Odası (\\d+)$", RegexOptions.CultureInvariant), "optional interior – entrance wardrobe $1"),
        (new Regex("^Opsiyonel İç Mekan – Vestiyer Odası$", RegexOptions.CultureInvariant), "optional interior – entrance wardrobe"),
        (new Regex("^Opsiyonel İç Mekan – Yatak Odası Banyo (\\d+)$", RegexOptions.CultureInvariant), "optional interior – bedroom bathroom $1"),
        (new Regex("^Opsiyonel İç Mekan – Yatak Odası Banyo$", RegexOptions.CultureInvariant), "optional interior – bedroom bathroom"),
        (new Regex("^Opsiyonel İç Mekan – Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "optional interior – bedroom $1"),
        (new Regex("^Opsiyonel İç Mekan – Yatak Odası$", RegexOptions.CultureInvariant), "optional interior – bedroom"),
        (new Regex("^Opsiyonel İç Mekan – Giriş (\\d+)$", RegexOptions.CultureInvariant), "optional interior – entrance $1"),
        (new Regex("^Opsiyonel İç Mekan – Giriş$", RegexOptions.CultureInvariant), "optional interior – entrance"),
        (new Regex("^Opsiyonel İç Mekan – Hol (\\d+)$", RegexOptions.CultureInvariant), "optional interior – hallway $1"),
        (new Regex("^Opsiyonel İç Mekan – Hol$", RegexOptions.CultureInvariant), "optional interior – hallway"),
        (new Regex("^Opsiyonel İç Mekan – Kiler (\\d+)$", RegexOptions.CultureInvariant), "optional interior – pantry $1"),
        (new Regex("^Opsiyonel İç Mekan – Kiler$", RegexOptions.CultureInvariant), "optional interior – pantry"),
        (new Regex("^Opsiyonel İç Mekan – Mutfak (\\d+)$", RegexOptions.CultureInvariant), "optional interior – kitchen $1"),
        (new Regex("^Opsiyonel İç Mekan – Mutfak$", RegexOptions.CultureInvariant), "optional interior – kitchen"),
        (new Regex("^Opsiyonel İç Mekan – Oda (\\d+)$", RegexOptions.CultureInvariant), "optional interior – room $1"),
        (new Regex("^Opsiyonel İç Mekan – Oda$", RegexOptions.CultureInvariant), "optional interior – room"),
        (new Regex("^Opsiyonel İç Mekan – Salon (\\d+)$", RegexOptions.CultureInvariant), "optional interior – living room $1"),
        (new Regex("^Opsiyonel İç Mekan – Salon$", RegexOptions.CultureInvariant), "optional interior – living room"),
        (new Regex("^Opsiyonel İç Mekan – WC (\\d+)$", RegexOptions.CultureInvariant), "optional interior – WC $1"),
        (new Regex("^Opsiyonel İç Mekan – WC$", RegexOptions.CultureInvariant), "optional interior – WC"),
        (new Regex("^İç Mekan – Zemin Kat – Misafir Odası Banyosu (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – guest bathroom $1"),
        (new Regex("^İç Mekan – Zemin Kat – Misafir Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – guest bedroom $1"),
        (new Regex("^İç Mekan – Zemin Kat – Salon & Mutfak (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – living room and kitchen $1"),
        (new Regex("^İç Mekan – Zemin Kat – Tuvalet (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – WC $1"),
        (new Regex("^İç Mekan – Zemin Kat – Ebeveyn Banyo (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – en-suite bathroom $1"),
        (new Regex("^İç Mekan – Zemin Kat – Ebeveyn Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – master bedroom $1"),
        (new Regex("^İç Mekan – Zemin Kat – Genel Banyo (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – family bathroom $1"),
        (new Regex("^İç Mekan – Zemin Kat – Yatak Odası \\(Balkonlu\\) (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – bedroom with balcony $1"),
        (new Regex("^İç Mekan – Zemin Kat – Çocuk Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – ground floor – children's room $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Misafir Odası Banyosu (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – guest bathroom $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Misafir Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – guest bedroom $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Salon & Mutfak (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – living room and kitchen $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Tuvalet (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – WC $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Ebeveyn Banyo (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – en-suite bathroom $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Ebeveyn Yatak Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – master bedroom $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Genel Banyo (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – family bathroom $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Yatak Odası \\(Balkonlu\\) (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – bedroom with balcony $1"),
        (new Regex("^İç Mekan – 1\\. Kat – Çocuk Odası (\\d+)$", RegexOptions.CultureInvariant), "interior – first floor – children's room $1"),
        (new Regex("^açık yüzme havuzu görünümü (\\d+)$", RegexOptions.CultureInvariant), "outdoor swimming pool view $1"),
        (new Regex("^açık yüzme havuzu görünümü$", RegexOptions.CultureInvariant), "outdoor swimming pool"),
        (new Regex("^açık çocuk oyun alanı görünümü (\\d+)$", RegexOptions.CultureInvariant), "outdoor children's playground view $1"),
        (new Regex("^açık çocuk oyun alanı görünümü$", RegexOptions.CultureInvariant), "outdoor children's playground"),
        (new Regex("^barbekü alanı görünümü (\\d+)$", RegexOptions.CultureInvariant), "barbecue area view $1"),
        (new Regex("^barbekü alanı görünümü$", RegexOptions.CultureInvariant), "barbecue area"),
        (new Regex("^basketbol sahası görünümü (\\d+)$", RegexOptions.CultureInvariant), "basketball court view $1"),
        (new Regex("^basketbol sahası görünümü$", RegexOptions.CultureInvariant), "basketball court"),
        (new Regex("^concierge alanı görünümü (\\d+)$", RegexOptions.CultureInvariant), "concierge area view $1"),
        (new Regex("^concierge alanı görünümü$", RegexOptions.CultureInvariant), "concierge area"),
        (new Regex("^fitness salonu görünümü (\\d+)$", RegexOptions.CultureInvariant), "gym view $1"),
        (new Regex("^fitness salonu görünümü$", RegexOptions.CultureInvariant), "gym"),
        (new Regex("^hamam görünümü (\\d+)$", RegexOptions.CultureInvariant), "Turkish bath view $1"),
        (new Regex("^hamam görünümü$", RegexOptions.CultureInvariant), "Turkish bath"),
        (new Regex("^kafeterya görünümü (\\d+)$", RegexOptions.CultureInvariant), "café view $1"),
        (new Regex("^kafeterya görünümü$", RegexOptions.CultureInvariant), "café"),
        (new Regex("^kapalı otopark görünümü (\\d+)$", RegexOptions.CultureInvariant), "indoor car park view $1"),
        (new Regex("^kapalı otopark görünümü$", RegexOptions.CultureInvariant), "indoor car park"),
        (new Regex("^kapalı yüzme havuzu görünümü (\\d+)$", RegexOptions.CultureInvariant), "indoor swimming pool view $1"),
        (new Regex("^kapalı yüzme havuzu görünümü$", RegexOptions.CultureInvariant), "indoor swimming pool"),
        (new Regex("^kapalı çocuk oyun alanı görünümü (\\d+)$", RegexOptions.CultureInvariant), "indoor children's play area view $1"),
        (new Regex("^kapalı çocuk oyun alanı görünümü$", RegexOptions.CultureInvariant), "indoor children's play area"),
        (new Regex("^kreş ve oyun odası görünümü (\\d+)$", RegexOptions.CultureInvariant), "nursery and playroom view $1"),
        (new Regex("^kreş ve oyun odası görünümü$", RegexOptions.CultureInvariant), "nursery and playroom"),
        (new Regex("^kuaför görünümü (\\d+)$", RegexOptions.CultureInvariant), "hairdresser view $1"),
        (new Regex("^kuaför görünümü$", RegexOptions.CultureInvariant), "hairdresser"),
        (new Regex("^spa alanı görünümü (\\d+)$", RegexOptions.CultureInvariant), "spa view $1"),
        (new Regex("^spa alanı görünümü$", RegexOptions.CultureInvariant), "spa"),
        (new Regex("^yüzme havuzu görünümü (\\d+)$", RegexOptions.CultureInvariant), "swimming pool view $1"),
        (new Regex("^yüzme havuzu görünümü$", RegexOptions.CultureInvariant), "swimming pool"),
        (new Regex("^dış cephe görünümü (\\d+)$", RegexOptions.CultureInvariant), "exterior view $1"),
        (new Regex("^dış cephe görünümü$", RegexOptions.CultureInvariant), "exterior view"),
        (new Regex("^dış mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "outdoor view $1"),
        (new Regex("^dış mekan görünümü$", RegexOptions.CultureInvariant), "outdoor view"),
        (new Regex("^iç mekan görünümü (\\d+)$", RegexOptions.CultureInvariant), "interior view $1"),
        (new Regex("^iç mekan görünümü$", RegexOptions.CultureInvariant), "interior view"),
        (new Regex("^sosyal alan görünümü (\\d+)$", RegexOptions.CultureInvariant), "social facilities view $1"),
        (new Regex("^sosyal alan görünümü$", RegexOptions.CultureInvariant), "social facilities view"),
        (new Regex("^görünümü (\\d+)$", RegexOptions.CultureInvariant), "view $1"),
    };

    public static async Task ApplyAsync(AppDbContext context)
    {
        var changed = false;

        var projects = await context.Projects
            .Include(p => p.ConceptImages)
            .Include(p => p.ConceptVideos)
            .Include(p => p.SitePlanImages)
            .Include(p => p.NearbyPlaces)
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .AsSplitQuery()
            .ToListAsync();

        var projectRevisions = Projects.ToLookup(r => r.Slug);
        var conceptImageRevisions = ConceptImages.ToDictionary(r => (r.Slug, r.TurkishTitle));
        var conceptVideoRevisions = ConceptVideos.ToDictionary(r => (r.Slug, r.TurkishTitle));
        var apartmentTypes = ApartmentTypes.ToDictionary(r => r.Turkish);
        var roomNames = RoomNames.ToDictionary(r => r.Turkish);
        var nearbyPlaceNames = NearbyPlaceNames.ToDictionary(r => r.Turkish);
        var sitePlanAltTexts = SitePlanAltTexts.ToDictionary(r => r.Turkish);

        foreach (var project in projects)
        {
            foreach (var revision in projectRevisions[project.Slug])
            {
                switch (revision.Field)
                {
                    case "ShortDescriptionEn":
                        project.ShortDescriptionEn = Revise(project.ShortDescriptionEn, revision.OldValue, revision.NewValue, ref changed);
                        break;
                    case "DescriptionEn":
                        project.DescriptionEn = Revise(project.DescriptionEn, revision.OldValue, revision.NewValue, ref changed);
                        break;
                    case "ConceptDescriptionEn":
                        project.ConceptDescriptionEn = Revise(project.ConceptDescriptionEn, revision.OldValue, revision.NewValue, ref changed);
                        break;
                    case "AmenitiesEn":
                        project.AmenitiesEn = Revise(project.AmenitiesEn, revision.OldValue, revision.NewValue, ref changed);
                        break;
                }
            }

            foreach (var image in project.ConceptImages)
            {
                if (conceptImageRevisions.TryGetValue((project.Slug, image.Title), out var revision))
                {
                    image.EyebrowEn = Revise(image.EyebrowEn, revision.OldEyebrow, revision.Eyebrow, ref changed);
                    image.TitleEn = Revise(image.TitleEn, revision.OldTitle, revision.Title, ref changed);
                    image.DescriptionEn = Revise(image.DescriptionEn, revision.OldDescription, revision.Description, ref changed);
                }
            }

            foreach (var video in project.ConceptVideos)
            {
                if (conceptVideoRevisions.TryGetValue((project.Slug, video.Title), out var revision))
                {
                    video.EyebrowEn = Revise(video.EyebrowEn, revision.OldEyebrow, revision.Eyebrow, ref changed);
                    video.TitleEn = Revise(video.TitleEn, revision.OldTitle, revision.Title, ref changed);
                    video.DescriptionEn = Revise(video.DescriptionEn, revision.OldDescription, revision.Description, ref changed);
                }
            }

            foreach (var floorPlan in project.FloorPlans)
            {
                if (apartmentTypes.TryGetValue(floorPlan.ApartmentType, out var typeRevision))
                {
                    floorPlan.ApartmentTypeEn = Revise(floorPlan.ApartmentTypeEn, typeRevision.OldValue, typeRevision.NewValue, ref changed);
                }

                foreach (var room in floorPlan.Rooms)
                {
                    if (roomNames.TryGetValue(room.Name, out var roomRevision))
                    {
                        room.NameEn = Revise(room.NameEn, roomRevision.OldValue, roomRevision.NewValue, ref changed);
                    }
                }
            }

            foreach (var place in project.NearbyPlaces)
            {
                if (nearbyPlaceNames.TryGetValue(place.Name, out var placeRevision))
                {
                    place.NameEn = Revise(place.NameEn, placeRevision.OldValue, placeRevision.NewValue, ref changed);
                }
            }

            foreach (var sitePlan in project.SitePlanImages)
            {
                if (sitePlanAltTexts.TryGetValue(sitePlan.AltText, out var altRevision))
                {
                    sitePlan.AltTextEn = Revise(sitePlan.AltTextEn, altRevision.OldValue, altRevision.NewValue, ref changed);
                }
            }

            foreach (var image in project.Images)
            {
                if (TranslateAltText(project.Name, image.AltText) is not { } altTextEn || image.AltTextEn == altTextEn)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(image.AltTextEn) ||
                    image.AltTextEn == TranslateAltText(project.Name, image.AltText, firstPass: true))
                {
                    image.AltTextEn = altTextEn;
                    changed = true;
                }
            }
        }

        var seoRevisions = Seo.ToDictionary(r => r.Page);
        foreach (var row in await context.SeoMetadata.ToListAsync())
        {
            if (seoRevisions.TryGetValue(row.Page, out var revision))
            {
                row.MetaTitleEn = Revise(row.MetaTitleEn, revision.OldTitle, revision.Title, ref changed);
                row.MetaDescriptionEn = Revise(row.MetaDescriptionEn, revision.OldDescription, revision.Description, ref changed);
            }
        }

        var settings = await context.SiteSettings.FirstOrDefaultAsync();
        if (settings is not null)
        {
            settings.WorkingHoursEn = Revise(settings.WorkingHoursEn, OldWorkingHoursEn, WorkingHoursEn, ref changed);
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Returns the new English text when the current value is empty or still
    // the exact previous English text; otherwise keeps the current value
    // (a deliberate later edit).
    private static string? Revise(string? current, string? oldValue, string newValue, ref bool changed)
    {
        var shouldReplace = string.IsNullOrWhiteSpace(current) ||
            (oldValue is not null && string.Equals(current, oldValue, StringComparison.Ordinal)) ||
            FirstPassTexts.Contains((current, newValue));

        if (!shouldReplace || string.Equals(current, newValue, StringComparison.Ordinal))
        {
            return current;
        }

        changed = true;
        return newValue;
    }

    // Public so the rules can be exercised in isolation; null when the alt
    // text does not start with the project's name or no rule matches. The
    // English text starts with the project's English display name ("La Fiore
    // Karabağ Phase 2"); firstPass reproduces the first pass's output (Turkish
    // name, earlier wording) so ApplyAsync can recognise and upgrade it.
    public static string? TranslateAltText(string projectName, string altText, bool firstPass = false)
    {
        var prefix = projectName + " ";
        if (!altText.StartsWith(prefix, StringComparison.Ordinal))
        {
            return null;
        }

        var englishPrefix = firstPass ? prefix : ProjectDisplayText.Name(projectName, isEnglish: true) + " ";
        var rest = altText[prefix.Length..];
        foreach (var (pattern, replacement) in AltTextRules)
        {
            if (pattern.IsMatch(rest))
            {
                var effectiveReplacement = firstPass && FirstPassAltTextReplacements.TryGetValue(pattern.ToString(), out var firstPassReplacement)
                    ? firstPassReplacement
                    : replacement;
                return englishPrefix + pattern.Replace(rest, effectiveReplacement);
            }
        }

        return null;
    }
}
