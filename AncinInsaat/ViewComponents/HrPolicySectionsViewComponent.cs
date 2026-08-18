using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// HR Policy content sections (docs/03_PageBlueprints.md — Page HR Policy).
// Folkart-reference revision (2026-08-17, project owner-approved): eight
// heading + bullet-list sections reproducing the reference "İK
// Politikamız" screenshot's structure, heading order and per-section
// bullet counts exactly — no cards, icons or numbering, plain uppercase
// navy headings over plain bullet lists per HrPolicySections/Default.cshtml.
// Static content, same hardcoded-copy precedent as CoreValuesViewComponent
// — HR Policy has exactly one set of these sections, so no calling page
// needs to pass data in.
//
// PLACEHOLDER copy — the client has not yet supplied the real HR policy
// text. Lorem Ipsum is used for every bullet per the project owner's
// explicit instruction not to invent Turkish company copy; only the eight
// section headings are real content, reproduced verbatim (already
// uppercase, matching the reference — not relying on CSS text-transform,
// which mis-cases the Turkish dotted/dotless İ/I pair unless the document
// language is set correctly).
public class HrPolicySectionsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new HrPolicySectionsViewModel
        {
            Sections = new List<HrPolicySectionItem>
            {
                new()
                {
                    Heading = "YETENEK KAZANIMI",
                    Bullets = new List<string>
                    {
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat, duis aute irure dolor in reprehenderit.",
                        "In voluptate velit esse cillum dolore eu fugiat nulla pariatur, excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae.",
                        "Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt, neque porro quisquam est."
                    }
                },
                new()
                {
                    Heading = "PERFORMANS YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur.",
                        "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati.",
                        "Et harum quidem rerum facilis est et expedita distinctio, nam libero tempore cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus."
                    }
                },
                new()
                {
                    Heading = "ÜCRETLENDİRME VE ÖDÜL YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae.",
                        "Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.",
                        "Curabitur pretium tincidunt lacus, nulla gravida orci a odio, nullam varius luctus pharetra, donec facilisis dolor auctor cursus commodo, vestibulum ac diam sit amet quam vehicula elementum.",
                        "Sed nisi, nulla quis sem at nibh elementum imperdiet, duis sagittis ipsum praesent mauris, fusce nec tellus sed augue semper porta mattis tincidunt nisi.",
                        "Mauris massa, vestibulum lacinia arcu eget nulla class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos, curabitur sodales ligula in libero.",
                        "Sed dignissim lacinia nunc, curabitur tortor pellentesque nibh aenean quam in scelerisque sem at dolor, maecenas mattis sed convallis tristique sem proin ut ligula vel nunc egestas porttitor."
                    }
                },
                new()
                {
                    Heading = "KURUMSAL GELİŞİM VE ÖĞRENME",
                    Bullets = new List<string>
                    {
                        "Morbi imperdiet, mauris ac auctor dictum, nisl ligula egestas nulla, et sollicitudin sem purus in lacus, nulla vulputate diam nec tempor nibh.",
                        "Aenean gravida nunc sed pede, cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus, donec vitae sapien ut libero venenatis faucibus, nullam quis ante etiam sit amet orci.",
                        "Eget eros faucibus tincidunt, duis leo, sed fringilla mauris sit amet nibh, donec sodales sagittis magna, sed consequat leo eget bibendum sodales augue velit cursus nunc.",
                        "Quisque velit nisi, pretium ut lacinia in, elementum id enim, curabitur non nulla sit amet nisl tempus convallis quis ac lectus.",
                        "Proin eget tortor risus, cras ultricies ligula sed magna dictum porta, vivamus suscipit tortor eget felis porttitor volutpat, praesent sapien massa convallis a pellentesque nec egestas non nisi."
                    }
                },
                new()
                {
                    Heading = "KARİYER YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Vivamus magna justo, lacinia eget consectetur sed, convallis at tellus, donec rutrum congue leo eget malesuada, curabitur arcu erat, accumsan id imperdiet et, porttitor at sem.",
                        "Nulla porttitor accumsan tincidunt, vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae, donec sollicitudin molestie malesuada, cras ultricies ligula sed magna dictum porta."
                    }
                },
                new()
                {
                    Heading = "LİDERLİK",
                    Bullets = new List<string>
                    {
                        "Pellentesque in ipsum id orci porta dapibus, curabitur non nulla sit amet nisl tempus convallis quis ac lectus, quisque velit nisi pretium ut lacinia in elementum id enim.",
                        "Vestibulum ac diam sit amet quam vehicula elementum sed sit amet dui, mauris blandit aliquet elit eget tincidunt nibh pulvinar a, sed porttitor lectus nibh."
                    }
                },
                new()
                {
                    Heading = "ÇEŞİTLİLİK VE KAPSAYICILIK",
                    Bullets = new List<string>
                    {
                        "Curabitur aliquet quam id dui posuere blandit, praesent sapien massa convallis a pellentesque nec egestas non nisi, cras ultricies ligula sed magna dictum porta.",
                        "Donec sollicitudin molestie malesuada, vivamus suscipit tortor eget felis porttitor volutpat, proin eget tortor risus cras ultricies ligula sed magna dictum porta.",
                        "Nulla quis lorem ut libero malesuada feugiat, curabitur non nulla sit amet nisl tempus convallis quis ac lectus, quisque velit nisi pretium ut lacinia in elementum id enim.",
                        "Sed porttitor lectus nibh, vestibulum ac diam sit amet quam vehicula elementum sed sit amet dui, mauris blandit aliquet elit eget tincidunt nibh pulvinar a."
                    }
                },
                new()
                {
                    Heading = "ETKİN İLETİŞİM VE İŞ BİRLİĞİ",
                    Bullets = new List<string>
                    {
                        "Lorem ipsum dolor sit amet consectetur adipiscing elit, praesent sapien massa convallis a pellentesque nec egestas non nisi, cras ultricies ligula sed magna dictum porta.",
                        "Vivamus magna justo lacinia eget consectetur sed convallis at tellus, donec rutrum congue leo eget malesuada, curabitur arcu erat accumsan id imperdiet et porttitor at sem.",
                        "Nulla porttitor accumsan tincidunt vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae, donec sollicitudin molestie malesuada.",
                        "Curabitur non nulla sit amet nisl tempus convallis quis ac lectus, quisque velit nisi pretium ut lacinia in elementum id enim, sed porttitor lectus nibh."
                    }
                }
            }
        };

        return View(model);
    }
}
